using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using Newtonsoft.Json;
using System.Collections.Generic; //For using List
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;


using Microsoft.Rest;
using System.Web;
using Bot_Application1.New_Classes;

namespace Bot_Application1
{

    [BotAuthentication] 
    public class MessagesController : ApiController
    {
        static int count = 0;

        private static string userName;
 

        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            //const string apiKey = "<'d88aa3de0ea4408bb2b1e329934daf00'>";
            //const string apiKey = "d88aa3de0ea4408bb2b1e329934daf00";

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));


            if (activity != null && activity.Type == ActivityTypes.Message)
            {
                StateClient stateClient = activity.GetStateClient();
                
                switch (count)
                {
                    case 0:
                        {
                            MoviesAdderIntoHashArray.initializeMovieListWorkbooks();
                            //MoviesSorter.sortMoviesHashTableAlphabetically();

                            await Conversation.SendAsync(activity, () => new FirstDialog());
                            count++;
                            break;
                        }
                    case 1:
                        {
                            userName = activity.Text;
                            await Conversation.SendAsync(activity, () => new FirstDialog());
                            count++;
                            BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);
                            userData.SetProperty<String>("userName", userName);
                            await stateClient.BotState.SetUserDataAsync(activity.ChannelId, activity.From.Id, userData);
                            break;
                        }
                    case 2:
                        await Conversation.SendAsync(activity, () => new FirstDialog());
                        break;               

                    default:
                        {                                                   
                            break;
                        }
                }
                  

            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

         

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;

        }


    }

    [Serializable]
    public class FirstDialog : IDialog<object>
    {
        protected int count = 0;
        private static List<string> intentsList = new List<string>();

        private static List<Movie> similarMovies;
        private static int j = 0;
        private static int k = 0;
        private static int hellCount = 0;
        private static int resetPromptCount = 0;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync); //wait() suspends the current dialog until the user has sent a message to the bot
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            
            var message = await argument;
            bool wait = true;
            bool switchCase = true;
            string[] titleResponses = {
                "Please tell me the title of a movie.",
                "What's the title of the movie?",
                "Name of the movie?",
                "Title of the movie?",
                "What's the name of the movie?"
            };


            if (message.Text.Contains("reset") || message.Text.Contains("Reset"))
            {
                wait = false;
                switchCase = false;

                if (resetPromptCount == 0)
                {
                    PromptDialog.Confirm(context,
                  AfterResetAsync1,
                  "Are you sure? Because I'm having a great time!",
                  "I didn't understand that.",
                  promptStyle: PromptStyle.None);
                    resetPromptCount++;
                }
                else
                {
                    PromptDialog.Confirm(context,
                 AfterResetAsync1,
                 "Sure about that?",
                 "I didn't understand that.",
                 promptStyle: PromptStyle.None);
                }

            }

            else if (message.Text.Contains("bye") || message.Text.Contains("Bye"))
            {
                switchCase = false;
                await context.PostAsync("Bye. Hope to see you again soon!");

            }
            else if (message.Text.Contains("thank you") || message.Text.Contains("Thank You") ||
                message.Text.Contains("Thanks") || message.Text.Contains("thanks") || message.Text.Contains("Thank you"))
            {
                switchCase = false;
                string[] respones = { "You're welcome :)", "No problem", "Anytime :)", "No worries", "Sure" };
                Random rand = new Random();
                int responseNo = rand.Next(0, 4);
                await context.PostAsync(respones[responseNo]);

            }

            else if (message.Text.Contains("go to hell") || message.Text.Contains("Go to hell") || message.Text.Contains("GO TO HELL")
                || (message.Text.Contains("go") || message.Text.Contains("Go")|| message.Text.Contains("GO")) && hellCount == 2)
            {
                switchCase = false;
                switch (hellCount)
                {
                    case 0:
                        await context.PostAsync("Not without you dear ;)");
                        hellCount++;
                        break;
                    case 1:
                        await context.PostAsync("I think I should. At least I won't have to worry about this stupid attendance policy there.");
                        hellCount++;
                        break;
                    case 2:
                        await context.PostAsync("Is this the only sentence you know?");
                        hellCount = 0;
                        break;

                }

            }


            else if (message.Text.Contains("highest rated") || message.Text.Contains("Highest rated"))
            {
                string[] hRatedResponses = {
                    "Sure. Which genre?",
                    "Please"
                };
                wait = false;
                switchCase = false;
                PromptDialog.Text(context,
                    AfterResetAsync2,
                    "Sure. Which genre?",
                    "I didn't understand that.");
            }

            else if (message.Text.Contains("similar") || message.Text.Contains("Similar"))
            {
                switchCase = false;
                Movie movie = returnMovie(message.Text, "to");

                j = 0;
                similarMovies = SimilarMoviesFinder.returnSimilarMoviesList(movie.getTitle());
                if (similarMovies != null)
                {
                    wait = false;

                    await context.PostAsync("Here are some movies similar to \"" + movie.getTitle() + "\".");

                    for (int i = 0; i < 5; i++, j++)
                    {
                        await context.PostAsync(similarMovies[j].getTitle() + "\n");
                    }

                    PromptDialog.Confirm(context,
                        AfterResetAsync,
                        "Would you like to see some more similar movies?",
                        "Sorry, didn't get that.",
                        promptStyle: PromptStyle.None);

                }                    
          
            }

            else if (message.Text.Contains("info") || message.Text.Contains("info"))
            {
                switchCase = false;
                Movie movie = returnMovie(message.Text, "of");

                if (movie != null)
                {                                                           
                    await context.PostAsync("Here's the general info of the movie \"" + movie.getTitle() + "\":\n");
                    if (movie.getDirector() != null)
                    {
                        await context.PostAsync("Director: "+movie.getDirector());
                    }
                    if (movie.getCast() != null)
                    {
                        await context.PostAsync("Cast: " + movie.getCast());
                    }
                    if (movie.getGenres() != null)
                    {
                        await context.PostAsync("Genre: " + movie.getGenres());
                    }
                    if (movie.getYearOfRelease() != null)
                    {
                        await context.PostAsync("Year of Release: " + movie.getYearOfRelease());
                    }
                }


                else
                    await context.PostAsync("Sorry I don't have the information regarding the cast of this movie.");
            }

            else if (message.Text.Contains("movie") || message.Text.Contains("Movie"))
            {
                switchCase = false;
                int index;
                
                if (message.Text.Contains("movie"))
                    index = message.Text.IndexOf("movie");
                else
                    index = message.Text.IndexOf("Movie");

                string movieTitle = message.Text.Substring(0, index - 1);

                j = 0;
                similarMovies = SimilarMoviesFinder.returnSimilarMoviesList(movieTitle);
                if (similarMovies != null)
                {
                    wait = false;

                    await context.PostAsync("Here are some movies similar to \"" + movieTitle + "\".");

                    for (int i = 0; i < 5; i++, j++)
                    {
                        await context.PostAsync(similarMovies[j].getTitle() + "\n");
                    }

                    PromptDialog.Confirm(context,
                        AfterResetAsync,
                        "Would you like to see some more similar movies?",
                        "Sorry, didn't get that.",
                        promptStyle: PromptStyle.None);

                }

                            

                else
                {
                    await context.PostAsync("Sorry, there are no movies similar to " + "\"" + movieTitle + "\" in my collection.");
                }

            }



            else if (message.Text.Contains("rating"))
            {
                switchCase = false;
                Movie movie = returnMovie(message.Text, "of");

                if (movie.getRating() != null)
                {
                    string[] res = {
                    "It's "+movie.getRating() + " on IMDB",
                     movie.getRating() + " it is.",
                    "This movie has a rating of " + movie.getRating() + " on IMDB",

                };

                    Random random = new Random();
                    int resNo = random.Next(0, 3);

                    await context.PostAsync(res[resNo]);
                }

                else
                    await context.PostAsync("Sorry I don't have the information regarding the rating of this movie.");


            }

            else if (message.Text.Contains("cast") || message.Text.Contains("Cast"))
            {
                switchCase = false;
                Movie movie = returnMovie(message.Text, "of");

                if (movie.getCast() != null)
                {
                    string[] res = {
                    "This movie has the following cast: \n"+movie.getCast(),
                     "Here it is: \n" + movie.getCast(),
                    "Here's the cast: \n"+movie.getCast(),
            };

                    Random random = new Random();
                    int resNo = random.Next(0, 3);

                    await context.PostAsync(res[resNo]);
                }
                else
                    await context.PostAsync("Sorry I don't have the information regarding the cast of this movie.");

            }

            else if (message.Text.Contains("director") || message.Text.Contains("Director"))
            {
                switchCase = false;
                Movie movie = returnMovie(message.Text, "of");

                if (movie.getDirector() != null)
                {
                    string[] res = {
                    "Here's the director of the movie: "+movie.getDirector(),
                     "Here it is: \n" + movie.getDirector(),
                    "Here's the director: \n"+movie.getDirector(),
            };

                    Random random = new Random();
                    int resNo = random.Next(0, 3);

                    await context.PostAsync(res[resNo]);
                }
                else
                    await context.PostAsync("Sorry I don't have the information regarding the director(s) of this movie.");
            }

            else if (message.Text.Contains("genre") || message.Text.Contains("genres") ||
                message.Text.Contains("Genre") || message.Text.Contains("Genres"))
            {
                switchCase = false;
                Movie movie = returnMovie(message.Text, "of");

                if (movie.getGenres() != null)
                {
                    string[] res = {
                    "Here's the genre of the movie: "+movie.getGenres(),                  
                    "Here's the genre: \n"+movie.getGenres(),
                    "Here it is: \n" + movie.getGenres()
            };

                    Random random = new Random();
                    int resNo = random.Next(0, 3);

                    await context.PostAsync(res[resNo]);
                }
                else
                    await context.PostAsync("Sorry I don't have the information regarding the genre of this movie.");
            }

            else if (message.Text.Contains("year of release") || message.Text.Contains("Year of release")
                || message.Text.Contains("Year of Release") || message.Text.Contains("Year Of Release"))
            {
                switchCase = false;
                Movie movie = returnMovie(message.Text, "of");

                if (movie.getYearOfRelease() != null)
                {
                    string[] res = {
                    "Here's the Year of Release of the movie: "+movie.getYearOfRelease(),
                    "Here's the Year of Release: \n"+movie.getYearOfRelease(),
                    "Here it is: \n" + movie.getYearOfRelease()
            };

                    Random random = new Random();
                    int resNo = random.Next(0, 3);

                    await context.PostAsync(res[resNo]);
                }
                else
                    await context.PostAsync("Sorry I don't have the information regarding the Year of Release of this movie.");
            }



            if (switchCase)
            {
                switch (count)
                {
                    case 0:
                        await context.PostAsync("Hi there! I am Cinebot. If you're a fan of movies, you would probably be talking to me a lot!");
                        await context.PostAsync("So first things first. What should I call you?");
                        count++;
                        break;
                    case 1:
                        await context.PostAsync("Hi " + message.Text+ "!");
                        await context.PostAsync("I can show you movies similar to the movie you tell me."
                            +" I can also show you the highest rated movies belonging to any genre and much more cool stuff!");
                        await context.PostAsync("Enough from me. It's your turn now!");
                        count++;
                        break;

                    case 2:
                        string[] responses = {
                            "Well, I don't know what to do with this information. Try typing something else.",
                            "Sorry, I've no idea what you're taking about. Try again please.",
                            "Didn't get that. Try again.",
                            "Sorry, I didn't understand that.",
                        };

                        Random random = new Random();
                        int responseNo = random.Next(0, 3);
                        await context.PostAsync(responses[responseNo]);
                        break;
                       

                }
            }
          

            if (wait)
            context.Wait(MessageReceivedAsync);

        }

        public async Task AfterResetAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            int moreSimilarCount = 0;
            var confirm = await argument;
            if (confirm)
            {
                await context.PostAsync("There you go.");
                for (int i = 0; i < 5; i++, j++)
                {
                    if (similarMovies[j] != null)
                    {
                        await context.PostAsync(similarMovies[j].getTitle() + "\n");
                        moreSimilarCount++;
                    }
                }

                if (moreSimilarCount == 0)
                    await context.PostAsync("Sorry, there are no more similar movies in my collection.");
            }
            else
            {
                await context.PostAsync("As you wish. But do remember that I have a great collection of movies! ;)");
            }

            context.Wait(MessageReceivedAsync);
        }

        public async Task AfterResetAsync1(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirm = await argument;
            if (confirm)
            {
                count = 0;
                await context.PostAsync("Conversation Resetted.");
            }
            else
            {
                await context.PostAsync("Good! Let's continue from where we left off.");
            }

            context.Wait(MessageReceivedAsync);
        }

        public async Task AfterResetAsync2(IDialogContext context, IAwaitable<string> argument)
        {
            var genre = await argument;
       
            await context.PostAsync("There you go.");

            List<Movie> moviesList =  HighestRatedMoviesFinder.findMovies(genre);

            if (moviesList != null)
            {
                for (int i = 0; i < 5; i++, k++)
                {                   
                    if (moviesList[k].getRating() != null)
                    {
                        await context.PostAsync(moviesList[k].getTitle() + ": " +moviesList[k].getRating());
                    }                 

                }
            }
            else
            {
                await context.PostAsync("No movies found");
            }


            context.Wait(MessageReceivedAsync);

        }

        public async Task AfterResetAsync3(IDialogContext context, IAwaitable<string> argument)
        {
            var title = await argument;
            bool w = true;
            j = 0;

            similarMovies = SimilarMoviesFinder.returnSimilarMoviesList(title);
            if (similarMovies != null)
            {
                w = false;

                await context.PostAsync("Here are some movies similar to \""+title+ "\".");

                for (int i = 0; i < 5; i++, j++)
                {
                    await context.PostAsync(similarMovies[j].getTitle() + "\n");
                }

                PromptDialog.Confirm(context,
                    AfterResetAsync,
                    "Would you like to see some more similar movies?",
                    "Sorry, didn't get that.",
                    promptStyle: PromptStyle.None);

            }


            else
            {
                await context.PostAsync("Sorry, there are no movies similar to " + "\"" + title + "\" in my collection.");
            }

            if (w)
            context.Wait(MessageReceivedAsync);

        }

        private static Movie returnMovie(string text, string s)
        {
            int movieIndex = text.IndexOf(s) + 2;
            string title = text.Substring(movieIndex).Trim();

            List<Movie> specificList = MoviesAdderIntoHashArray.getSpecificListFromMoviesHashTable(((int)Char.ToUpper(title[0])) - 65);
            Movie movie = null;

            foreach (Movie m in specificList)
            {
                if (m.getTitle().Trim().Equals(title.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    movie = m;
                    break;
                }
            }

            return movie;
        }

        
    }

}









