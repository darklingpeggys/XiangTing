using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlueBird_Plugin;

namespace BlueBirdFriend
{
    class Handle
    {
        Random random = new Random(DateTime.Now.Millisecond + DateTime.Now.Second * 1000);
        Dictionary<string, Character> characters = new Dictionary<string, Character>();
        public Dictionary<string, Character> characterList { get { return characters; } }
        public Handle()
        {

            Thread thread = new Thread(new ThreadStart(Auto));
            thread.Start();
        }
        public void AddCharacter(Character character)
        {
            lock (characters)
            {
                characters.Add(character.character_id, character);
            }
        }
        public void DelCharacter(string id)
        {
            lock (characters)
            {
                characters.Remove(id);
            }
        }
        void Auto()
        {
            while (true)
            {
                ITask task = TaskQueue.GetTask();
                if (task == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (task.handle_info.type==ITask.HALDLE_TYPE.ALL_WITHOUT)
                {
                    foreach (KeyValuePair<string, Character> character in characters)
                    {
                        if (!task.handle_info.without_id.Contains(character.Value.character_id))
                        {
                            character.Value.AddTask(task);
                        }
                    }
                }
                else if(task.handle_info.type ==ITask.HALDLE_TYPE.ALL)
                {
                    foreach (KeyValuePair<string, Character> character in characters)
                    {
                        character.Value.AddTask(task);
                    }
                }
                else if (task.handle_info.type == ITask.HALDLE_TYPE.RANDOM)
                {
                    KeyValuePair<string, Character>[] character_list = characters.ToArray();
                    character_list[random.Next(0, character_list.Length)].Value.AddTask(task);
                }
                else if (task.handle_info.type == ITask.HALDLE_TYPE.RANDOM_MUL)
                {
                    KeyValuePair<string, Character>[] character_list = characters.ToArray();
                    if (character_list.Length > 0)
                    {
                        for (int i = 0; i < task.handle_info.count; i++)
                        {
                            character_list[random.Next(0, character_list.Length)].Value.AddTask(task);
                        }
                    }
                }
                else if (task.handle_info.type == ITask.HALDLE_TYPE.RANDOM_WITHOUT)
                {
                    List<KeyValuePair<string, Character>> trash = new List<KeyValuePair<string, Character>>();
                    List<KeyValuePair<string, Character>> character_list = characters.ToList();
                    for(int i = 0; i < character_list.Count; i++)
                    {
                        if (task.handle_info.without_id.Contains(character_list[i].Value.character_id))
                        {
                            trash.Add(character_list[i]);
                        }
                    }
                    foreach(KeyValuePair<string, Character> i in trash)
                    {
                        character_list.Remove(i);
                    }
                    if (character_list.Count > 0)
                    {
                        character_list[random.Next(0, character_list.Count)].Value.AddTask(task);
                    }
                }
                else if (task.handle_info.type == ITask.HALDLE_TYPE.RANDOM_MUL_WITHOUT)
                {
                    List<KeyValuePair<string, Character>> trash = new List<KeyValuePair<string, Character>>();
                    List<KeyValuePair<string, Character>> character_list = characters.ToList();
                    for (int i = 0; i < character_list.Count; i++)
                    {
                        if (task.handle_info.without_id.Contains(character_list[i].Value.character_id))
                        {
                            trash.Add(character_list[i]);
                        }
                    }
                    foreach (KeyValuePair<string, Character> i in trash)
                    {
                        character_list.Remove(i);
                    }
                    if (character_list.Count > 0)
                    {
                        for(int i = 0; i < task.handle_info.count; i++)
                        {
                            character_list[random.Next(0, character_list.Count)].Value.AddTask(task);
                        }
                    }
                }
                else if (task.handle_info.type == ITask.HALDLE_TYPE.TARGET)
                {
                    characters[task.handle_info.id].AddTask(task);
                }
            }
        }
    }
}
