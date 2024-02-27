using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RLNETConsoleGame.Systems
{
    //this reps a queue of messages and output to a RLConsole which will be the message Console
    public class MessageLog
    {
        private static readonly int _maxLines = 9;      //Max lines to store

        private readonly Queue<string> _lines;          //Queue will keep track of messages (FIFO)

        public MessageLog()
        {
            _lines = new Queue<string>();
        }

        public void Add(string message)
        {
            _lines.Enqueue(message);            //this adds lines/ messages to teh queue

            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();               // this removes if messages / lines exceed the _maxLines of 9 
            }
        }

        public void Draw(RLConsole console)
        {
            console.Clear();
            string[] lines = _lines.ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                console.Print(1, i + 1, lines[i], RLColor.White);
            }
        }
    }
}
