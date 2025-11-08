using CodeForge3.PokerFace.Configurations;
using CodeForge3.PokerFace.Entities;
using CodeForge3.PokerFace.Enums;
using CodeForge3.PokerFace.MachineLearning.Interfaces;
using CodeForge3.PokerFace.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace CodeForge3.PokerFace.Services.Implementations;

/// <summary>
/// The implementation of the <see cref="IPokerAppService" />.
/// </summary>
public sealed class PokerAppService
    : IPokerAppService
{
    #region Fields
    
    /// <summary>
    /// The field containing the logger for the class.
    /// </summary>
    private readonly ILogger<PokerAppService> _logger;
    
    /// <summary>
    /// The field containing the Yolo image detection handler.
    /// </summary>
    private readonly IYoloDetectionHandler _yoloDetectionHandler;
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PokerAppService" /> class.
    /// </summary>
    /// <param name="logger">The logger for the class.</param>
    /// <param name="yoloDetectionHandler">The Yolo image detection handler.</param>
    public PokerAppService(ILogger<PokerAppService> logger, IYoloDetectionHandler yoloDetectionHandler)
    {
        _logger = logger;
        _yoloDetectionHandler = yoloDetectionHandler;
    }
    
    #endregion
    
    #region PredictCards
    
    /// <inheritdoc />
    public async Task<IReadOnlyList<CardPrediction>> PredictCardsAsync(IBrowserFile? file)
    {
        _logger.LogDebug("Starting card prediction.");
        
        if (file == null)
        {
            _logger.LogWarning("The file is null.");
            throw new ArgumentNullException(nameof(file));
        }
        
        await using Stream stream = file.OpenReadStream(PokerFaceConfiguration.MaxUploadFileSize);
        using MemoryStream ms = new();
        await stream.CopyToAsync(ms);
        byte[] imageBytes = ms.ToArray();
        
        IReadOnlyList<CardPrediction> predictions = await _yoloDetectionHandler.DetectAsync(imageBytes);
        
        _logger.LogInformation("Card prediction completed.");
        return predictions;
    }

    #endregion

    #region Evaluate

    /// inheritdoc />
    public ECardCombination EvaluateCombination(IReadOnlyList<Card> cards)
    {
        if (cards.Count > 5)
        {
            throw new ArgumentException($"Card evauation requires exactly 5 cards. Got: {cards.Count}.");
        }
        bool hasDuplicates = cards
            .Select(c => new { c.Rank, c.Suit })
            .Distinct()
            .Count() != cards.Count;
        if (hasDuplicates) 
        {
            throw new ArgumentException($"Duplicated cards.");
        }

        var rankCounts = Enum.GetValues<ECardRank>().ToDictionary(r => r, _ => 0);
        var suitCounts = Enum.GetValues<ECardSuit>().ToDictionary(s => s, _ => 0);
        int rankMask = 0;

        foreach (var card in cards)
        {
            rankCounts[card.Rank]++;
            suitCounts[card.Suit]++;
            rankMask |= 1 << ((int)card.Rank - 2); // -2 is because of the Enum-Rank-Two, which is the first element, but starts at 2 istead of 0.
        }


        bool flush = suitCounts.Values.Any(c => c == 5);


        var straightBitMask = 0b11111;
        bool straight = Enumerable.Range(0, 9).Any(shift => ((rankMask >> shift) & straightBitMask) == straightBitMask);

        var lowAceStraightBitMask = 0b1000000001111; // (A-2-3-4-5)
        if (!straight && (rankMask & lowAceStraightBitMask) == lowAceStraightBitMask)
            straight = true;


        int four = rankCounts.Values.Count(c => c == 4);
        int three = rankCounts.Values.Count(c => c == 3);
        int pairs = rankCounts.Values.Count(c => c == 2);


        if (straight && flush)
        {
            var royalFlushBitMask = 0b1111100000000;
            if ((rankMask & royalFlushBitMask) == royalFlushBitMask)
                return ECardCombination.RoyalFlush;

            return ECardCombination.StraightFlush;
        }

        if (four == 1) { return ECardCombination.FourOfAKind; }

        if (three == 1 && pairs == 1) { return ECardCombination.FullHouse; }

        if (flush) { return ECardCombination.Flush; }

        if (straight) { return ECardCombination.Straight; }

        if (three == 1) { return ECardCombination.ThreeOfAKind; }

        if (pairs == 2) { return ECardCombination.TwoPair; }

        if (pairs == 1) { return ECardCombination.Pair; }

        return ECardCombination.HighCard;
    }

    #endregion
}
