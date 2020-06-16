using System.Drawing;
using Tulpep.NotificationWindow;

namespace CSGOCheat.Helper_and_ImportDLL
{
    class Notifier
    {
        public static void Notif(string notif_text)
        {
            Form1.notifier = new PopupNotifier();
            Form1.notifier.Image = Properties.Resources.NotifierImage;
            Form1.notifier.ImageSize = new Size(96, 96);

            Form1.notifier.TitleColor = Color.Purple;
            Form1.notifier.HeaderColor = Color.Red;
            Form1.notifier.BodyColor = Color.DarkGray;

            Form1.notifier.TitleText = Variable.CheatName;
            Form1.notifier.ContentText = notif_text;

            Form1.notifier.Popup();
        }
    }
}
