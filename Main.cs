using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace ServerRestrictor
{
    public class MQSPlugin : RocketPlugin<Config>

    {
        public static MQSPlugin Instance;
        public UnityEngine.Color MessageColor { get; private set; }


        protected override void Load()
        {
            {
                MQSPlugin.Instance = this;
                MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, UnityEngine.Color.red);
                ChatManager.onChatted += onChat;
                U.Events.OnPlayerConnected += OnPlayerConnected;
                UnturnedPlayerEvents.OnPlayerInventoryAdded += OnInventoryAdd;
                UnturnedPlayerEvents.OnPlayerWear += OnWear;
                VehicleManager.onEnterVehicleRequested += onEnterVehicleRequested;
                Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
                Logger.LogWarning($"[{Name}] has been loaded! ");
                Logger.LogWarning("Dev: MQS#7816");
                Logger.LogWarning("Join this Discord for Support: https://discord.gg/Ssbpd9cvgp");
                Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            }
        }

        protected override void Unload()
        {
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            Logger.LogWarning($"[{Name}] has been unloaded! ");
            Logger.LogWarning("++++++++++++++++++++++++++++++++++++++");
            ChatManager.onChatted -= onChat;
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            UnturnedPlayerEvents.OnPlayerInventoryAdded -= OnInventoryAdd;
            UnturnedPlayerEvents.OnPlayerWear -= OnWear;
            VehicleManager.onEnterVehicleRequested -= onEnterVehicleRequested;
        }


        private void onChat(SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            var message = Configuration.Instance.RestrictedWords.FirstOrDefault(w => text.ToLower().Contains(w.name.ToLower()));

            var converted = UnturnedPlayer.FromSteamPlayer(player);
            {
                if (player.isAdmin && Configuration.Instance.IgnoreAdmins || converted.GetPermissions().Any(x => x.Name == "ignore.*"))
                {
                    return;
                }

                if (Configuration.Instance.RestrictedWords.Any(w => text.ToLower().Contains(w.name.ToLower())))
                {
                    if (converted.GetPermissions().Any(x => x.Name == message.Ignore))
                    {
                        return;
                    }

                    else
                    {
                        if (message.Message == null)
                        {
                            ChatManager.serverSendMessage(MQSPlugin.Instance.Translate("WordBlacklisted"), MessageColor, null, player, EChatMode.SAY, "", true);
                            Logger.Log(MQSPlugin.Instance.Translate("ConsoleWordBlacklistLog", converted.DisplayName));
                        }

                        else
                        {
                            UnturnedChat.Say(converted, message.Message.Replace("!NAME!", converted.DisplayName), MessageColor, true);
                            Logger.Log(message.Message.Replace("!NAME!", converted.DisplayName));
                        }
                        isVisible = false;
                    }
                }
            }
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            var playername = player.CharacterName;

            var name = Configuration.Instance.RestrictedNames.FirstOrDefault(n => playername.ToLower().Contains(n.name.ToLower()));


            if (player.IsAdmin && Configuration.Instance.IgnoreAdmins || player.GetPermissions().Any(x => x.Name == "ignore.*"))
            {
                return;
            }

            if (Configuration.Instance.RestrictedNames.Any(n => playername.ToLower().Contains(n.name.ToLower())))
            {
                if (player.GetPermissions().Any(x => x.Name == name.Ignore))
                {
                    return;
                }

                else
                {
                    if (name.Message == null)
                    {
                        player.Kick(MQSPlugin.Instance.Translate("NameBlacklist"));
                        Logger.Log(MQSPlugin.Instance.Translate("ConsoleNameBlacklistKickLog", player.DisplayName));
                    }

                    else
                    {
                        player.Kick(name.Message.Replace("!NAME!", playername));
                        Logger.Log(name.Message.Replace("!NAME!", playername));
                    }

                }
            }
        }



        private void OnInventoryAdd(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P)
        {
            var item = Configuration.Instance.RestrictedItems.FirstOrDefault(x => x.ItemId == P.item.id);

            if (player.IsAdmin && Configuration.Instance.IgnoreAdmins || player.GetPermissions().Any(x => x.Name == "ignore.*"))
            {
                return;
            }

            if (Configuration.Instance.RestrictedItems.Any(i => i.ItemId == P.item.id))
            {
                if (player.GetPermissions().Any(x => x.Name == item.Ignore))
                {
                    return;
                }

                else
                {
                    player.Inventory.removeItem((byte)inventoryGroup, inventoryIndex);
                    if (item.Message == null)
                    {
                        UnturnedChat.Say(player, MQSPlugin.Instance.Translate("ItemBlacklisted"), MessageColor, true);
                        Logger.Log(MQSPlugin.Instance.Translate("ConsoleItemBlacklistLog", player.DisplayName));
                    }

                    else
                    {
                        UnturnedChat.Say(player, item.Message.Replace("!NAME!", player.DisplayName), MessageColor, true);
                        Logger.Log(item.Message.Replace("!NAME!", player.DisplayName));
                    }
                }
            }
        }


        private void onEnterVehicleRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            var car = Configuration.Instance.RestrictedVehicles.FirstOrDefault(x => x.VehicleId == vehicle.id);

            var driver = UnturnedPlayer.FromPlayer(player);

            if (driver.IsAdmin && Configuration.Instance.IgnoreAdmins || driver.GetPermissions().Any(x => x.Name == "ignore.*"))
            {
                return;
            }

            if (Configuration.Instance.RestrictedVehicles.Any(x => x.VehicleId == vehicle.id))
            {
                if (driver.GetPermissions().Any(x => x.Name == car.Ignore))
                {
                    return;
                }

                else
                {
                    shouldAllow = false;

                    if (car.Message == null)
                    {
                        UnturnedChat.Say(driver, MQSPlugin.Instance.Translate("VehicleBlacklist"), MessageColor, true);
                        Logger.Log(MQSPlugin.Instance.Translate("ConsoleVehicleBlacklistLog", driver.DisplayName));
                    }

                    else
                    {
                        UnturnedChat.Say(driver, car.Message.Replace("!NAME!", driver.DisplayName), MessageColor, true);
                        Logger.Log(car.Message.Replace("!NAME!", driver.DisplayName));
                    }
                }
            }
        }

        private void OnWear(UnturnedPlayer player, UnturnedPlayerEvents.Wearables wear, ushort id, byte? quality)
        {
            var item = Configuration.Instance.RestrictedItems.FirstOrDefault(x => x.ItemId == id);

            if (player.IsAdmin && Configuration.Instance.IgnoreAdmins || player.GetPermissions().Any(x => x.Name == "ignore.*"))
            {
                return;
            }

            if (Configuration.Instance.RestrictedItems.Any(x => x.ItemId == id))
            {
                if (player.GetPermissions().Any(x => x.Name == item.Ignore))
                {
                    return;
                }

                else
                {

                    switch (wear)
                    {
                       
                        case UnturnedPlayerEvents.Wearables.Backpack:
                            StartCoroutine(InvokeOnNextFrame(() =>
                            player.Player.clothing.askWearBackpack(0, 0, new byte[0], false)));
                            break;
                        case UnturnedPlayerEvents.Wearables.Glasses:
                            StartCoroutine(InvokeOnNextFrame(() =>
                            player.Player.clothing.askWearGlasses(0, 0, new byte[0], false)));
                            break;
                        case UnturnedPlayerEvents.Wearables.Hat:
                            StartCoroutine(InvokeOnNextFrame(() =>
                            player.Player.clothing.askWearHat(0, 0, new byte[0], false)));
                            break;
                        case UnturnedPlayerEvents.Wearables.Mask:
                            StartCoroutine(InvokeOnNextFrame(() =>
                            player.Player.clothing.askWearMask(0, 0, new byte[0], false)));
                            break;
                        case UnturnedPlayerEvents.Wearables.Pants:
                            StartCoroutine(InvokeOnNextFrame(() =>
                            player.Player.clothing.askWearPants(0, 0, new byte[0], false)));
                            break;
                        case UnturnedPlayerEvents.Wearables.Shirt:
                            StartCoroutine(InvokeOnNextFrame(() =>
                            player.Player.clothing.askWearShirt(0, 0, new byte[0], false)));
                            break;
                        case UnturnedPlayerEvents.Wearables.Vest:
                            StartCoroutine(InvokeOnNextFrame(() =>
                            player.Player.clothing.askWearVest(0, 0, new byte[0], false)));
                            break;
                    }
                }
            }
        }

        private IEnumerator InvokeOnNextFrame(System.Action action)
        {
            yield return new WaitForFixedUpdate();
            action();
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            { "WordBlacklisted", "You said a word that is on the blacklist. Please moderate yourself!" },
            { "ItemBlacklisted", "You are not allowed to loot that item." },
            { "NameBlacklist", "Your name is in the Blacklist. Please change it." },
            { "VehicleBlacklist", "You are not allowed to drive this vehicle. The vehicle is in the Blacklist."},
            { "ConsoleNameBlacklistKickLog", "{0} was kicked because his name is in the Name Blacklist." },
            { "ConsoleWordBlacklistLog", "{0} tried to write a Blacklisted Word." },
            { "ConsoleItemBlacklistLog", "{0} tried to loot a Blacklisted Item." },
            { "ConsoleVehicleBlacklistLog", "{0} tried to drive a Blacklisted vehicle."}
        };
    }
}