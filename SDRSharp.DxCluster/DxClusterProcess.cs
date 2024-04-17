using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDRSharp.Common;
using SDRSharp.Radio;
using SDRSharp.PanView;
using System.Drawing;
using static SDRSharp.PanView.BitmapHelper;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SDRSharp.DxCluster
{
    public partial class DxClusterProcess
    {
        private ISharpControl _control;
        private DxClusterClient _clusterClient;
        private bool pluginEnable;

        public bool clusterConnected;

        public DxClusterProcess(ISharpControl control) 
        {
            _control = control;
            //_control.PropertyChanged += _control_PropertyChanged;

            _control.SpectrumAnalyzerCustomPaint += SpectrumAnalyzerCustomPaint;
            _control.Perform();

        }

        public void connectCluster(string mycallsign)
        {
            _clusterClient = new DxClusterClient(mycallsign);
            pluginEnable = true;
        }

        public void disconnectCluster()
        {
            if (_clusterClient != null)
            {
                _clusterClient.disconnect();
            }
            pluginEnable = false;
        }

        private void SpectrumAnalyzerCustomPaint(object sender, CustomPaintEventArgs e)
        {
            if(pluginEnable==true)
            {
                clusterConnected = _clusterClient.connected;

                if (_clusterClient != null)
                {

                    SpectrumAnalyzer _spectrum = (SpectrumAnalyzer)sender;
                    Font _spectrumFreqDataFont = new Font(FontFamily.GenericSansSerif, 8f);
                    int y_fontSize = (int)e.Graphics.MeasureString("X", _spectrumFreqDataFont).Height;
                    int x_fontSize = (int)e.Graphics.MeasureString("X", _spectrumFreqDataFont).Width;
                    int y_offset_tobelow = (int)Math.Round(y_fontSize * 2.5);
                    //int last_y_position = y_fontSize;
                    int y_position = 0;

                    SolidBrush _spectrumFreqInfoLineBrush = new SolidBrush(Color.FromArgb(100, Color.GreenYellow));
                    SolidBrush _spectrumFreqInfoTextBrush = new SolidBrush(Color.FromArgb(200, Color.White));
                    StringFormat _spectrumFreqDataTextFormat = new StringFormat(StringFormat.GenericTypographic);


                    for (int i = 0; i < _clusterClient.lastEntries.Count; i++)
                    {
                        var entry = _clusterClient.lastEntries[i];

                        try
                        {
                            double dxfreq = Convert.ToDouble(entry["frequency"]) * 1000;
                            int x_position = (int)_spectrum.FrequencyToPoint(dxfreq);

                            int spectrum_height = ((Control)(object)_spectrum).Height;
                            int spectrum_width = ((Control)(object)_spectrum).Width;

                            //double x_frequency_min = _spectrum.PointToFrequency(0);
                            //double x_frequency_max = _spectrum.PointToFrequency(spectrum_width);

                            int y_fromTop = spectrum_height;



                            // ======= Random Y from top to prevent overlap callsign
                            if (entry["y_position"] == "") // process only frequency not update
                            {
                                //List<float> numbers = new List<float> { 2, 3, 4, 5, 6, 7, 8 }; // Ensure at least one operand is a float
                                //Random random = new Random();
                                //int randomIndex = random.Next(0, numbers.Count);
                                //int y_position = (int)Math.Round(numbers[randomIndex]); // Round to the nearest integer

                                //y_position = y_fromTop / y_position;


                                // Check if y overlaped, then try to move position to belop 1 font size.
                                //if (checkIfFrequencyOverlap(y_position, y_fontSize) == true)
                                //{
                                if(checkIfFrequencyOverlap(dxfreq, entry["callsign"]) == true) // same band and diff 1khz
                                {
                                    y_position = getLastY_positionSameBand(dxfreq) + (y_fontSize);
                                }
                                else
                                {
                                    y_position = y_fontSize;
                                }
                                    
                                //}


                                _clusterClient.lastEntries[i]["y_position"] = Convert.ToString(y_position); // save y position
                                _clusterClient.lastEntries[i]["x_position"] = Convert.ToString(x_position); // save x position

                                //y_fromTop = y_fromTop / y_position;
                                y_fromTop = y_position;
                            }
                            else // If Y_position already save, then read back
                            {
                                y_position = Convert.ToInt32(entry["y_position"]);
                                //y_fromTop = y_fromTop / y_position;
                                y_fromTop = y_position;                        
                            }
                            // ======= End Random Y


                            Point point = default(Point);
                            Point point_start_fromTop = new Point(x_position, point.Y + y_fromTop);

                                                       

                            e.Graphics.DrawLine(new Pen(_spectrumFreqInfoLineBrush), point_start_fromTop, new Point(point_start_fromTop.X, point_start_fromTop.Y + spectrum_height - point_start_fromTop.Y - y_offset_tobelow));

                            // Draw Bacground
                            var textPosition = new Point(point_start_fromTop.X + 5, point_start_fromTop.Y);
                            var size = e.Graphics.MeasureString(entry["callsign"], _spectrumFreqDataFont);
                            var rect = new RectangleF(textPosition.X, textPosition.Y, size.Width, size.Height);
                            e.Graphics.FillRectangle(Brushes.BlueViolet, rect);

                            // Draw arrow head
                            int arrowWidth = x_fontSize;
                            int arrowHeight = y_fontSize;
                            int arrowX = x_position;
                            int arrowY = point_start_fromTop.Y + spectrum_height - point_start_fromTop.Y - (int)(y_offset_tobelow *1.7);
                            Point[] points = new Point[3];
                            points[0] = new Point(arrowX - arrowWidth / 2, arrowY + arrowHeight);
                            points[1] = new Point(arrowX + arrowWidth / 2, arrowY + arrowHeight);
                            points[2] = new Point(arrowX, arrowY + arrowHeight + arrowWidth);
                            e.Graphics.FillPolygon(Brushes.YellowGreen, points);


                            e.Graphics.DrawString(entry["callsign"], _spectrumFreqDataFont, _spectrumFreqInfoTextBrush, new Point(point_start_fromTop.X + 5, point_start_fromTop.Y), _spectrumFreqDataTextFormat);
                            //last_y_position = y_position; // save last y_position for next round cacluate
                        }
                        catch
                        { }

                    }
                }
            }

            else 
            {
                if (_clusterClient != null)
                {
                    clusterConnected = _clusterClient.connected;
                }
            }


        }


        private bool checkIfFrequencyOverlap(double curr_dxfrequency, string curr_callsign)
        {
            bool ret = false;

            for (int i = 0; i < _clusterClient.lastEntries.Count; i++)
            {
                var entry = _clusterClient.lastEntries[i];

                //int.TryParse(entry["y_position"], out int entry_y_position);
                double.TryParse(entry["frequency"], out double entry_frequency);

                if (entry_frequency != 0)
                {


                    entry_frequency = entry_frequency * 1000;

                    double frequency_diff = Math.Abs(curr_dxfrequency - entry_frequency);

                    if (curr_dxfrequency == entry_frequency) // same frequency but difference call
                    {
                        if (curr_callsign != entry["callsign"])
                        {
                            ret = true; break;
                        }
                    }
                    else if (frequency_diff < 5000) // overlap frequency > 1 MHz
                    {

                        // Is same band ?
                        int entry_frequency_band = (int)entry_frequency / 1000000;
                        int dx_freq_band = (int)curr_dxfrequency / 1000000;
                        if (entry_frequency_band == dx_freq_band && entry["y_position"] != "")
                        {
                            ret = true; break;
                        }
                    }
                }
            }

            return ret;
        }

        

        private int getLastY_positionSameBand(double dx_freq)
        {
            int ret = 0;

            for (int i = 0; i < _clusterClient.lastEntries.Count; i++)
            {
                var entry = _clusterClient.lastEntries[i];
                double.TryParse(entry["frequency"], out double entry_frequency);
                entry_frequency = entry_frequency * 1000;


                int entry_frequency_band = (int)entry_frequency / 1000000;
                int dx_freq_band = (int)dx_freq / 1000000;
                if (entry_frequency_band == dx_freq_band && entry["y_position"] !="")
                {
                    int.TryParse(entry["y_position"], out int last_y_position);
                    ret = last_y_position;
                }
            }

            return ret; ;
        }

        //private bool checkIfFrequencyOverlap(int dx_y_position, int y_fontsize)
        //{
        //    bool ret = false;

        //    for (int i = 0; i < _clusterClient.lastEntries.Count; i++)
        //    {
        //        var entry = _clusterClient.lastEntries[i];

        //        if(entry["y_position"] != "")
        //        {
        //            int entry_y_position = Convert.ToInt32(entry["y_position"]);
        //            double y_position_diff = Math.Abs(entry_y_position - dx_y_position);
        //            if (y_position_diff < y_fontsize) // Check if frequency diff < 1 kHz, mean overlap
        //            {
        //                ret = true; break;
        //            }
        //        }
                
        //    }
        //        return ret;
        //}
    
    }
}
