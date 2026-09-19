using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.PreLoadCard;

public interface IPreLoadCard2Command
{
    void Fill(CardProfile cardProfile, Response2.PreLoadCard preLoadCard);
}

public abstract class BasePreLoadCard2Command : IPreLoadCard2Command
{
    public virtual void Fill(CardProfile cardProfile, Response2.PreLoadCard preLoadCard)
    {
        if (preLoadCard.load_player != null) 
            Fill(cardProfile, preLoadCard.load_player);
            
        if (preLoadCard.User != null) 
            Fill(cardProfile, preLoadCard.User);
            
        if (preLoadCard.MatchingTag != null) 
            Fill(cardProfile, preLoadCard.MatchingTag);
    }
    
    public virtual void Fill(CardProfile cardProfile, Response2.PreLoadCard.LoadPlayer loadPlayer) 
    {
        // optional override 
    }
    
    public virtual void Fill(CardProfile cardProfile, Response2.PreLoadCard.MobileUserGroup mobileUserGroup) 
    {
        // optional override 
    }
    
    public virtual void Fill(CardProfile cardProfile, MatchingTag matchingTag) 
    {
        // optional override 
    }
}