using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using VK_UI3.Helpers;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Model.RequestParams.Leads;
using VkNet.Utils;

namespace VK_UI3.VKs.IVK
{
    public class UserAudio : IVKGetAudio
    {
        public UserAudio(long id, DispatcherQueue dispatcher) : base(id, dispatcher)
        {
            base.id = id.ToString();
        }

        public override long? getCount()
        {
            return api.Audio.GetCountAsync(long.Parse(id)).Result;
        }

        User user;
        public override string getName()
        {
            try
            {
                List<long> ids = new List<long> { long.Parse(id) };
                user = api.Users.GetAsync(ids).Result[0];

                var request = new VkParameters
                {
                    {"user_ids", string.Join(",", ids)},
                    {"fields", string.Join(",", "photo_max_orig")}
                };

                var response = api.Call("users.get", request);
                user = User.FromJson(response[0]);


                return user.FirstName + " " + user.LastName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }


        public override Uri getPhoto()
        {
            if (user != null)
            {
                return
                    user.PhotoMaxOrig;
            }
            else
                return null;
        }

        public override void GetTracks()
        {
            if (getLoadedTracks) return;
            getLoadedTracks = true;

            Task.Run(async () =>
            {
                int offset = listAudio.Count;
                int count = 500;

                if (countTracks > listAudio.Count)
                {
                    VkCollection<Audio> audios;

                    
                    audios = api.Audio.GetAsync(new AudioGetParams
                    {
                        OwnerId = int.Parse(id),
                        Offset = offset,
                        Count = count
                    }).Result;


                    ManualResetEvent resetEvent = new ManualResetEvent(false);

                    foreach (var item in audios)
                    {
                        ExtendedAudio extendedAudio = new ExtendedAudio(item, this);

                        DispatcherQueue.TryEnqueue(() =>
                        {
                            listAudio.Add(extendedAudio);
                            resetEvent.Set(); // ������������� � ���������� ������
                        });

                        resetEvent.WaitOne(); // ������� ������� � ���������� ������
                        resetEvent.Reset(); // ���������� ������� ��� ��������� ��������
                    }

                    if (countTracks == listAudio.Count()) itsAll = true;


                    getLoadedTracks = false;
                }
                NotifyOnListUpdate();
            });
        }

    }
}
