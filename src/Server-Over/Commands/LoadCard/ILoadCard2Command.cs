using nue.protocol.exvs;
using ServerOver.Models.Cards;

namespace ServerOver.Commands.LoadCard;

public interface ILoadCard2Command
{
    void Fill(CardProfile cardProfile, Response2.LoadCard loadCard);
}

public abstract class BaseLoadCard2Command : ILoadCard2Command
{
    public virtual void Fill(CardProfile cardProfile, Response2.LoadCard loadCard)
    {
        if (loadCard.mobile_user_group != null) 
            Fill(cardProfile, loadCard.mobile_user_group);
            
        if (loadCard.pilot_data_group != null) 
            Fill(cardProfile, loadCard.pilot_data_group);
    }
    
    public virtual void Fill(CardProfile cardProfile, Response2.LoadCard.MobileUserGroup mobileUserGroup)
    {
        // optional override 
    }
    
    public virtual void Fill(CardProfile cardProfile, Response2.LoadCard.PilotDataGroup pilotDataGroup)
    {
        // optional override 
    }
}