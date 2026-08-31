using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.LoadCard;

public interface ILoadCard2Command
{
    void Fill(CardProfile cardProfile, Response2.LoadCard loadCard);
}