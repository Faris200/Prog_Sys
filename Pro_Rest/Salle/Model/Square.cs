using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Salle.Controller;

namespace Salle.Model
{
    public class Square : SquareInterface
    {
        private int _idSquare;
        public int IdSquare
        {
            get => this._idSquare;
            set
            {
                if (value > 0)
                {
                    this._idSquare = value;
                }
            }
        }

        private int _NbClients;
        public int NbClients
        {
            get => this._NbClients;
            set
            {
                if (value >= 0)
                {
                    this._NbClients = value;
                }
            }
        }

        private List<LineInterface> _LineList;
        public List<LineInterface> LineList
        {
            get => this._LineList;
            set => this._LineList = value;
        }

        private HeadWaiterInterface _headWaiter;
        public HeadWaiterInterface headWaiter
        {
            get => this._headWaiter;
            set => this._headWaiter = value;
        }

        private List<WaiterInterface> _WaiterList;
        public List<WaiterInterface> WaiterList
        {
            get => this._WaiterList;
            set => this._WaiterList = value;
        }


        public Square(int idSquare)
        {
            this.IdSquare = idSquare;
            this.NbClients = 0;

            LineList = new List<LineInterface>();
            WaiterList = new List<WaiterInterface>();

            initialiseSquare(idSquare);
        }

        private void initialiseSquare(int idSquare)
        {
            if (idSquare == 1)
            {
                this.headWaiter = MaîtreHôtel.maîtreHôtelInstance().ListHeadWaiter[0];

                this.LineList.Add(new Line(1));
                this.LineList.Add(new Line(2));

                this.WaiterList.Add((WaiterInterface)new Waiter(1));
                this.WaiterList.Add((WaiterInterface)new Waiter(2));
            }
            else
            {
                this.headWaiter = MaîtreHôtel.maîtreHôtelInstance().ListHeadWaiter[1];

                this.LineList.Add(new Line(3));
                this.LineList.Add(new Line(4));

                this.WaiterList.Add((WaiterInterface)new Waiter(3));
                this.WaiterList.Add((WaiterInterface)new Waiter(4));
            }
        }

        public WaiterInterface GetFreeWaiter()
        {
            WaiterInterface Waiter = null;
            while (Waiter == null)
            {
                if (!WaiterList[0].Busy)
                {
                    Waiter = WaiterList[0];
                }
                else if (!WaiterList[1].Busy)
                {
                    Waiter = WaiterList[1];
                }
                else
                {
                    Thread.Sleep(30);
                }
            }
            return Waiter;
        }
    }
}
