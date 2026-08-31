using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.PreLoadCard;

public interface IPreLoadCardCommand
{
    void Fill(CardProfile cardProfile, Response.PreLoadCard preLoadCard);
}

public interface IPreLoadCard2Command
{
    void Fill(CardProfile cardProfile, Response2.PreLoadCard preLoadCard);
}