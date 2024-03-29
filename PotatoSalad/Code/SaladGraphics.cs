﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PotatoSalad
{
    class SaladGraphics
    {
        // This class exists to ease any future swapping out of display options.

        public void RenderText(string textToRender)
        {
            ListBox cOutput = Game.MainForm.consoleOutput;
            cOutput.Items.Insert(0, textToRender);
            Game.MainForm.consoleOutput.Refresh();
        }

        public void BlankMap()
        {
            Game.MainForm.BlankMap();
        }

        public void InfoText(string textToRender)
        {
            TextBox cOutput = Game.MainForm.informationBox;
            cOutput.ReadOnly = false;
            cOutput.Text = textToRender;
            cOutput.ReadOnly = true;
            Game.MainForm.informationBox.Refresh();
        }

        public void DrawMap(Map m)
        {
            Game.MainForm.DrawMap(m);
        }

        public void CursorDrawMap(Map m, PotatoSalad.Code.Cursor c)
        {
            Game.MainForm.CursorDrawMap(m, c);
        }

        public void RemoveMob(int x, int y, Map m)
        {
            // At the moment this blanks the whole square,
            // including items. That needs fixing in the called
            // function though.
            Game.MainForm.EraseMob(x, y, m);
        }

        public string CapitaliseString(string s)
        {
            return s.First().ToString().ToUpper() + s.Substring(1);
        }
    }
}
