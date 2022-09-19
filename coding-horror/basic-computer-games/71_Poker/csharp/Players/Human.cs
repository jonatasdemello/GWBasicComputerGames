using Poker.Cards;
using Poker.Strategies;

namespace Poker.Players;

internal class Human : Player
{
    private readonly IReadWrite _io;

    public Human(int bank, IReadWrite io)
        : base(bank)
    {
        HasWatch = true;
        _io = io;
    }

    public bool HasWatch { get; set; }

    protected override void DrawCards(Deck deck)
    {
        var count = _io.ReadNumber("How many cards do you want", 3, "You can't draw more than three cards.");
        if (count == 0) { return; }

        _io.WriteLine("What are their numbers:");
        for (var i = 1; i <= count; i++)
        {
            Hand = Hand.Replace((int)_io.ReadNumber(), deck.DealCard());
        }

        _io.WriteLine("Your new hand:");
        _io.Write(Hand);
    }

    internal bool SetWager()
    {
        var strategy = _io.ReadHumanStrategy(Table.Computer.Bet == 0 && Bet == 0);
        if (strategy is Strategies.Bet or Check)
        {
            if (Bet + strategy.Value < Table.Computer.Bet)
            {
                _io.WriteLine("If you can't see my bet, then fold.");
                return false;
            }
            if (Balance - Bet - strategy.Value >= 0)
            {
                HasBet = true;
                Bet += strategy.Value;
                return true;
            }
            RaiseFunds();
        }
        else
        {
            Fold();
            Table.CollectBets();
        }
        return false;
    }

    public void RaiseFunds()
    {
        _io.WriteLine();
        _io.WriteLine("You can't bet with what you haven't got.");

        if (Table.Computer.TryBuyWatch()) { return; }

        // The original program had some code about selling a tie tack, but due to a fault
        // in the logic the code was unreachable. I've omitted it in this port.

        IsBroke = true;
    }

    public void ReceiveWatch()
    {
        // In the original code the player does not pay any money to receive the watch back.
        HasWatch = true;
    }

    public void SellWatch(int amount)
    {
        HasWatch = false;
        Balance += amount;
    }

    public override void TakeWinnings()
    {
        _io.WriteLine("You win.");
        base.TakeWinnings();
    }
}
