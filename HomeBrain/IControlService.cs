using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HomeBrain
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IControlService" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IControlService
    {
        [OperationContract]
        Status GetStatus();
    }
}
