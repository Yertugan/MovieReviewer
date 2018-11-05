using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace PageAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        int actorScore;
        int scenarioScore;
        int musicScore;
        string moviegood;
        int effectsScore;
        double stars;
        string reviews;
        string acting, effects, music, plot, plot2, music2, recomm;
        List<string> reviewList = new List<string>();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        string MoviewTitle;
        string goodas, badas, goodms, badms, goodefs, badefs, goodpls, badpls;

        /// <summary>
        /// Get all reviews of movie from imdb.com
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>reviews</returns>
        protected string GetReview(string movie)
        {
            
            
            string UrlId = "https://www.imdb.com/find?ref_=nv_sr_fn&q=" + movie + "&s=all";
            HtmlWeb webId = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docId = webId.Load(UrlId);
            // var node = doc.DocumentNode.SelectNodes("//div[@class='text show-more__control']");
            var movieID = docId.DocumentNode.SelectSingleNode("//td[@class='result_text']");
            string[] parts = movieID.InnerHtml.Split('/');
            //textBox2.Text = parts[2];
            string UrlReview = "http://www.imdb.com/title/" + parts[2] + "/reviews?ref_=tt_urv";
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument docReview = webId.Load(UrlReview);
            var movieReview = docReview.DocumentNode.SelectNodes("//div[@class='text show-more__control']");
            string result = "";
            foreach(var m in movieReview)
            {
                result += m.InnerText;
                reviewList.Add(m.InnerText);
            }
            return result;
        }
        
        /// <summary>
        /// Button launch GetResult and GetReview methods and input results to labels and textbox2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Refresh();
            textBox2.Refresh();
            label1.Refresh();
            label2.Refresh();
            label3.Refresh();
            label4.Refresh();
            reviews = GetReview(MoviewTitle);
            
            GetResult(reviews);
            label1.Text = actorScore.ToString();
            label2.Text = scenarioScore.ToString();
            label3.Text = musicScore.ToString();
            label4.Text = effectsScore.ToString();
            string shortReview = "I want to give " + MoviewTitle + " " +  Math.Round(stars, 2) + " stars out of 10." + " First of all, an actor play was " + acting + 
                ". There is no doubt that the scenario is " + plot + ", which made this movie " + plot2 +
            ". The musical soundtrack was " + music + ". " + music2 + 
            ". The last but not least is visual effects. In this movie visual effects were " + effects + 
            ". All things considered, I would like to " + recomm +" to watch this " + moviegood + " movie.";
            textBox2.Text = shortReview;
        }

        /// <summary>
        /// Count stars, actore score, effects score, music score, plot score. 
        /// </summary>
        /// <param name="allReviews"></param>
        public void GetResult(string allReviews)
        {
            int cnta = 1, cnts = 1, cntm = 1, cnte = 1;
            int goodact = 0, badact = 0, goodp = 0, badp = 0, goodeff = 0, badeff = 0, goodmus = 0, badmus = 0;
            actorScore = 1;
            scenarioScore = 1;
            musicScore = 1;
            effectsScore = 1;
            stars = 1;
            string[] words = allReviews.Split(' ', '.', ',', '?', '!');
            string[] actorKey = { "actor", "role", "actors", "artist", "performer", "player", "actioner" , "play"};
            string[] scenarioKey = { "scenario", "plot", "plots", "storyline", "story", "action", "thread", "diegesis" };
            string[] musicKey = { "melody", "singing", "sound", "soundtrack", "sound studio", "track", "song","musical", "songs", "singing", "sing", "music"};
            string[] effectsKey = { "special effects", "visual effects", "effects", "effect", "animation", "camerawork", "computer graphics", "graphics" ,"visual", "graphic"};
            string[] goods = { "good", "cool","superb","pretty","interesting","super","spectatcular", "excellent","fancy", "awesome", "great", "lovely", "love", "amazing", "perfect", "brilliant", "fantastic", "sparkling", "crowd-pleasing", "magnificent", "attractive","masterpiece","fresh", "beautiful" };
            string[] bads = { "bad","not good", "awfull", "shit","ugly","crap","unworthy", "horrible", "cheep", "poor", "rough", "sad", "unacceptable", "lousy", "cheesy","pathetic","predictable", "fake", "dumb","dissapointing", "annoying", "sappy" };
            Dictionary<string[], string[]> openWith = new Dictionary<string[], string[]>();

            for (int i = 0; i < words.Length; i++)
            {
                for (int k = 0; k < goods.Length; k++)
                {
                    for (int l = 0; l < bads.Length; l++)
                    {
                        for (int j = 0; j < actorKey.Length; j++)
                        {
                            if (words[i] == actorKey[j])
                            {
                                if ((words[i + 1] == goods[k]) || (words[i - 1] == goods[k]) || (words[i + 2] == goods[k]) || words[i - 2] == goods[k])
                                {

                                    actorScore += 10;
                                    goodact++;
                                    cnta++;

                                    //textBox2.Text += goods[k];
                                }
                                if ((words[i + 1] == bads[l]) || (words[i - 1] == bads[l]) || (words[i + 2] == bads[l]) || words[i - 2] == bads[l])
                                {
                                    actorScore += 1;
                                    cnta++;
                                    badact++;
                                    //textBox2.Text += bads[l];
                                }
                            }
                        }
                        for (int j = 0; j < scenarioKey.Length; j++)
                        {
                            if (words[i] == scenarioKey[j])
                            {
                                if ((words[i + 1] == goods[k]) || (words[i - 1] == goods[k]) || (words[i + 2] == goods[k]) || words[i - 2] == goods[k])
                                {

                                    scenarioScore += 10;
                                    goodp++;
                                    cnts++;

                                }
                                if ((words[i + 1] == bads[l]) || (words[i - 1] == bads[l]) || (words[i + 2] == bads[l]) || words[i - 2] == bads[l])
                                {
                                    scenarioScore += 1;
                                    cnts++;
                                    badp++;
                                }
                            }
                        }

                        for (int j = 0; j < musicKey.Length; j++)
                        {
                            if (words[i] == musicKey[j])
                            {
                                if ((words[i + 1] == goods[k]) || (words[i - 1] == goods[k]) || (words[i + 2] == goods[k]) || words[i - 2] == goods[k])
                                {

                                    musicScore += 10;
                                    goodmus++;
                                    cntm++;

                                }
                                if ((words[i + 1] == bads[l]) || (words[i - 1] == bads[l]) || (words[i + 2] == bads[l]) || words[i - 2] == bads[l])
                                {
                                    musicScore += 1;
                                    cntm++;
                                    badmus++;
                                }
                            }
                        }

                        for (int j = 0; j < effectsKey.Length; j++)
                        {
                            if (words[i] == effectsKey[j])
                            {
                                if ((words[i + 1] == goods[k]) || (words[i - 1] == goods[k]) || (words[i + 2] == goods[k]) || words[i - 2] == goods[k])
                                {

                                    effectsScore += 10;
                                    goodeff++;
                                    cnte++;

                                }
                                if ((words[i + 1] == bads[l]) || (words[i - 1] == bads[l]) || (words[i + 2] == bads[l]) || words[i - 2] == bads[l])
                                {
                                    effectsScore += 1;
                                    cnte++;
                                    badeff++;
                                }
                            }
                        }
                    }
                }
            }


            actorScore = actorScore / cnta +1;
            scenarioScore = scenarioScore / cnts +1;
            musicScore = musicScore / cntm +1;
            effectsScore = effectsScore / cnte +1;

            stars = (actorScore + scenarioScore + musicScore + effectsScore) / 4 ;

            int maincnt = 4;

           
            if (goodact > badact) acting = "great";
            else if (badact > goodact) acting = "bad";
            else acting = "not good not bad";

            if (goodp > badp) { plot = "spectacular"; plot2 = "more interesting"; }
            else if (badact > goodact) { plot = "poor"; plot2 = "very boring"; }
            else { plot = "okay"; plot2 = "regular"; }

            if (goodmus > badmus) { music = "amazing"; music2 = "I would like to add that soundtrack to my playlist"; }
            else if (badmus > goodmus) { music = "not really good"; music2 = "I do not want to hear that music again"; }
            else { music = "normal"; music2 = "I think I won't add that soundrack to my playlist"; }

            if (goodeff > badeff) effects = "wonderful";
            else if (badeff > goodeff) effects = "cheep";
            else effects = "common";
            if(cnta == 1)
            {
                actorScore = 0;
                maincnt--;
                stars = (actorScore + scenarioScore + musicScore + effectsScore) / maincnt;
                acting = "not really memorable";
                actorScore = 0;
            }
            if(cnte == 1)
            {
                effectsScore = 0;
                maincnt--;
                stars = (actorScore + scenarioScore + musicScore + effectsScore) / maincnt;
                effects = "not much";
                effectsScore = 0;
            }

            if(cntm == 1)
            {
                musicScore = 0;
                maincnt--;
                stars = (actorScore + scenarioScore + musicScore + effectsScore) / maincnt;
                music = "not main part";
               
            }

            if (stars > 5)
            {
                recomm = "recommend";
                moviegood = "good";
            }
            else
            {
                moviegood = "bad";
                recomm = "not recommend";
            }
        }
        /// <summary>
        /// User input movie title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            MoviewTitle = textBox1.Text;
        }
    }
}
