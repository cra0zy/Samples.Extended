using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Samples.Extended.Samples;

namespace Samples.Extended
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // TODO: We could use reflection to populate this list automatically perhaps.
            var samples = new []
            {
                new SampleMetadata("Bitmap Fonts", () => new BitmapFontsSample()),
                new SampleMetadata("Sprites", () => new SpritesSample()),
                new SampleMetadata("Input Listeners", () => new InputListenersSample()),
                new SampleMetadata("Camera2D", () => new Camera2DSample())
            }
            .OrderBy(i => i.Name)
            .ToArray();

            // ReSharper disable once CoVariantArrayConversion
            SampleListBox.Items.AddRange(samples);
            SampleListBox.SelectedIndex = 0;
        }

        private class SampleMetadata
        {
            public SampleMetadata(string name, Func<Game> createSampleFunction)
            {
                Name = name;
                CreateSampleFunction = createSampleFunction;
            }

            public string Name { get; private set; }
            public Func<Game> CreateSampleFunction { get; private set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private Func<Game> _selectedSampleFunction; 

        public Game CreateSample()
        {
            return _selectedSampleFunction();
        }

        private void LaunchButton_Click(object sender, EventArgs e)
        {
            var sampleMetadata = SampleListBox.SelectedItem as SampleMetadata;

            if (sampleMetadata != null)
            {
                _selectedSampleFunction = sampleMetadata.CreateSampleFunction;
                DialogResult = DialogResult.OK;
            }
        }

    }
}
