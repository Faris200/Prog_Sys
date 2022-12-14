using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Kitchen.Controller;
using Salle.View;

namespace Kitchen.Sockets
{
    class CutleryDesk : InterfaceCutleryDesk
    {
        private Thread _thEcoute;
        private Affichage afficher;
        private static CutleryDesk CutleryDeskInstance;
        private DishWasherInterface DWasher;

        private byte[] _bytes;
        public byte[] bytes
        {
            get => this._bytes;
            set => this._bytes = value;
        }

        private Socket _s;
        public Socket s
        {
            get => this._s;
            set => this._s = value;
        }


        public static CutleryDesk cutleryDeskInstance()
        {
            if (CutleryDeskInstance == null)
            {
                CutleryDeskInstance = new CutleryDesk();
            }
            return CutleryDeskInstance;
        }


        private CutleryDesk()
        {
            this.DWasher = new DishWasher();
            afficher = new Affichage();
            _thEcoute = new Thread(new ThreadStart(EcouterCutleryDesk));
            new Pause().AddThread(_thEcoute);
            _thEcoute.Start();
        }

        public void EcouterCutleryDesk()
        { 

            //On crée le serveur en lui spécifiant le port sur lequel il devra écouter.
            UdpClient serveur = new UdpClient(5038);

            //Création d'une boucle infinie qui aura pour tâche d'écouter.
            while (true)
            {
                //Création d'un objet IPEndPoint qui recevra les données du Socket distant.
                IPEndPoint client = null;
                afficher.afficherLine("CutlerySocket's Socket Listening....");

                //On écoute jusqu'à recevoir un message.
                byte[] data = serveur.Receive(ref client);
                afficher.afficherLine("Cutlery received from the Waiter");

                //Décryptage et affichage du message.
                string message = Encoding.Default.GetString(data);
                afficher.afficherLine( message + " pieces of Cutlery and one piece of Laundry were given from the Waiter\n");

                DWasher.GetCutlery(message);

            }

        }

        public void SendDataCutleryDesk(int nb, int type)
        {
            try
            {
                // Sending message 
                //<Client Quit> is the sign for end of data 
                string theMessageToSend = type + ":" + nb;

                if(type == 0)
                {
                    afficher.afficherLine("The DishWasher is placing " +nb+" clean cutlery on the Cutlery Desk");
                }
                else
                {
                    afficher.afficherLine("The DishWasher is placing " + nb + " clean laundry in the Laundry stock");
                }

                byte[] msg = Encoding.Unicode.GetBytes(theMessageToSend);

                UdpClient udpClient = new UdpClient();
                udpClient.Send(msg, msg.Length, "127.0.0.1", 5037);
                //  udpClient.Send(msg, msg.Length, "10.144.50.44", 5037); 
                udpClient.Close();

            }
            catch (Exception exc)
            {
                afficher.afficherLine(exc.ToString());
            }
        }



    }
}
