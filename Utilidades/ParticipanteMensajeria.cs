//Agregamos referencia
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace QF.Utilidades
{
    //Pasamos a public y heredamos de ValueChangedMessage y le decimos que envie ParticipanteMessage
    public class ParticipanteMensajeria : ValueChangedMessage<ParticipanteMensaje>
    {
        //Creamos el constructor y le decimos que toda clase que use èsta reciba el valor de participanteMensaje
        public ParticipanteMensajeria(ParticipanteMensaje value):base(value) {
        } 

    }
}
