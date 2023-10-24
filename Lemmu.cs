using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using System;
using System.Collections.Generic;

namespace Lemmu
{
    public class Lemmu : PhysicsGame
    {
        //private bool peliKaynnissa = false;

        private readonly Image taustakuva = LoadImage("tausta");
        private readonly Image hahmonKuva = LoadImage("lemmu_1");
        private readonly Image ruuanKuva = LoadImage("heina_1");

        Vector paikkaRuudulla;

        private const int KENTANLEVEYS = 1000;
        private const int KENTANKORKEUS = 800;

        private int nalka = 100;
        private IntMeter nalkaLaskuri;
        //private int jano  = 0;
        //private int mieliala = 50;
        //private int puhtaus  = 100;

        private Timer timer;


        public override void Begin()
        {
            SetWindowSize(KENTANLEVEYS, KENTANKORKEUS);

            AloitaPeli();


            PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        }

        public void AloitaPeli()
        {
            LuoKentta();
            //peliKaynnissa = true;
        }

        public void LuoKentta()
        {
            Level.Background.Image = taustakuva;


            PhysicsObject hahmo = LuoHahmo();
            PhysicsObject ruoka = LuoSyotava();

            Mouse.IsCursorVisible = true;
            //paikkaRuudulla = Mouse.PositionOnScreen;
            //Mouse.Listen(MouseButton.Left, ButtonState.Pressed, Toiminto, "Toiminto", paikkaRuudulla);

            timer = new Timer();
            timer.Interval = 5;
            timer.Timeout += LisaaNalkaa;
            timer.Start();

            Mouse.ListenOn(ruoka, MouseButton.Left, ButtonState.Down, Liiku, null, ruoka);
            Mouse.ListenOn(ruoka, MouseButton.Left, ButtonState.Released, Syo, null, ruoka);

            LuoNalkaLaskuri();
            AddCollisionHandler(ruoka, "ruoka", TormaaHahmoon);
        }


        public void TormaaHahmoon(PhysicsObject syoja, PhysicsObject syotava)
        {
            syotava.Destroy();
            nalkaLaskuri.AddValue(-5);           
        }



        public PhysicsObject LuoHahmo()
        {
            PhysicsObject hahmo = new PhysicsObject(440.0, 368.0);
            hahmo.IgnoresCollisionResponse = true;
            hahmo.IgnoresGravity = true;
            hahmo.X = -150;
            hahmo.Y = 20;
            hahmo.Image = hahmonKuva;
            Add(hahmo);
            return hahmo;
        }


        void Syo(PhysicsObject ruoka)
        {
            ruoka.Destroy();
            nalkaLaskuri.Value += 10;
        }

        void Liiku(PhysicsObject ruoka)
        {
            Listener hiirenKuuntelija = Mouse.ListenMovement(0.1, delegate ()
            {
                ruoka.Position = Mouse.PositionOnWorld;

            }, "");
        }

        public PhysicsObject LuoSyotava()
        {
            PhysicsObject ruoka = new PhysicsObject(200.0, 68.0);
            ruoka.IgnoresCollisionResponse = true;
            ruoka = new PhysicsObject(200, 63);
            ruoka.X = 30;
            ruoka.Y = -130;
            ruoka.Image = ruuanKuva;
            Add(ruoka);
            return ruoka;
        }


        void LuoNalkaLaskuri()
        {
            nalkaLaskuri = new IntMeter(nalka);
            nalkaLaskuri.MaxValue = 100;
            nalkaLaskuri.MinValue = 0;
            // TODO: if nälkä = 100 --> peli loppui


            Label nayttoNalka = new Label();
            nayttoNalka.X = Screen.Left + 60;
            nayttoNalka.Y = Screen.Top - 30;
            nayttoNalka.Color = Color.White;
            nayttoNalka.TextColor = Color.Black;
            nayttoNalka.BindTo(nalkaLaskuri);
            nayttoNalka.Title = "Nälkä: ";
            Add(nayttoNalka);
        }


        void LisaaNalkaa()
        {
            nalkaLaskuri.Value -= 1;
        }
    }
}