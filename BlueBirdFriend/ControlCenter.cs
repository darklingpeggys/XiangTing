using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueBird_Plugin;

namespace BlueBirdFriend
{
    class ControlCenter
    {
        public Handle handle;
        public ControlCenter()
        {
            SendBirdTool.GetHome = SendBird.GetHome;
            SendBirdTool.GetNewTagTwitter = SendBird.GetNewTagTwitter;
            SendBirdTool.GetTweet = SendBird.GetTweet;
            SendBirdTool.MakeFollow = SendBird.MakeFollow;
            SendBirdTool.MakeLike = SendBird.MakeLike;
            SendBirdTool.MakeReplyBird = SendBird.MakeSimpleBird;
            SendBirdTool.MakeRetweet = SendBird.MakeRetweet;
            SendBirdTool.MakeSample = SendBird.MakeSample;
            SendBirdTool.MakeSimpleBird = SendBird.MakeSimpleBird;
            SendBirdTool.GetProfile = SendBird.GetProfile;

            TaskQueueTool.AddImportantTask = TaskQueue.AddImportantTask;
            TaskQueueTool.AddTask = TaskQueue.AddTask;
            TaskQueueTool.GetTask = TaskQueue.GetTask;

            handle = new Handle();
        }

        public void AddCharacter(string[] l)
        {
            if (l.Length < 4)
            {
                return;
            }
            Character character = new Character(new BirdAuth(l[0], l[1], l[2], l[3]));
            handle.AddCharacter(character);
            Tool.Log("添加了 " + character.character_id);
            foreach(KeyValuePair<string,Character> c in handle.characterList.ToArray())
            {
                if (!c.Value.alive)
                {
                    handle.characterList.Remove(c.Key);
                }
            }
        }

        public void UpdateProfile()
        {
            foreach (KeyValuePair<string, Character> c in handle.characterList.ToArray())
            {
                if (!c.Value.alive)
                {
                    handle.characterList.Remove(c.Key);
                }
                else
                {
                    c.Value.UpdateProfile();
                }
            }
        }
    }
}
