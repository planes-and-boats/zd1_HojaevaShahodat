using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace zd23_Hojaeva
{
    struct Song
    {
        public string Author;
        public string Title;
        public string Filename;
        public Song(string author, string title, string filename)
        {
            Author = author;
            Title = title;
            Filename = filename;
        }
    }
    class Playlist
    {
        private List<Song> list;        //скрытый лист песен
        private int currentIndex;       //индекс текущей песни

        public Playlist()       //конструктор
        {
            list = new List<Song>();
            currentIndex = 0;
        }

        public Song CurrentSong()       //метод, возвращающий текущую песню
        {
            if (list.Count > 0)
                return list[currentIndex];
            else
                throw new IndexOutOfRangeException(
                    "Невозможно получить текущую аудиозапись для пустого плейлиста!");
        }
        public void AddTrack(Song song)     //метод для добавления песни
        {
            //проверка на повторы
            bool exists = list.Exists(s => s.Author == song.Author &&
            s.Title == song.Title && s.Filename == song.Filename);
            if (exists) return;
            list.Add(song);
        }
        public void AddTrack(string author, string title, string filename)     //перегрузка метода для добавления песни
        {
            //проверка на повторы
            bool exists = list.Exists(s => s.Author == author &&
            s.Title == title && s.Filename == filename);
            if (exists) return;
            list.Add(new Song(author, title, filename));
        }
        public void Next()      //метод вперед
        {
            if (list.Count == 0) return;
            currentIndex = (currentIndex + 1) % list.Count;
        }
        public void Previous()      //метод назад
        {
            if (list.Count == 0) return;
            currentIndex = (currentIndex - 1 + list.Count) % list.Count;
        }
        public void ToStart()   //метод старт (переход к началу плейлиста)
        {
            currentIndex = 0;
        }
        public void GoTo(int index)
        {
            if (index < 0 || index >= list.Count)
            {
                throw new IndexOutOfRangeException(
                    "Индекс находится вне границ плейлиста!");
            }
            else currentIndex = index;
        }
        public void Clear()     //метод для очищения плейлиста
        {
            list.Clear();
            currentIndex = 0;
        }
        public bool Remove(int index)     //метод удаления композиции по индексу
        {
            if (index < 0 || index >= list.Count) return false;
            list.RemoveAt(index);
            if (list.Count == 0) currentIndex = 0;
            else if (currentIndex >= list.Count) currentIndex = list.Count - 1;
            return true;
        }
        public bool Remove(Song song)       //перегрузка метода удаления композиции по значению
        {
            int index = list.FindIndex(s => s.Author == song.Author &&
            s.Title == song.Title && s.Filename == song.Filename);
            if (index == -1) return false;
            list.RemoveAt(index);
            if (list.Count == 0) currentIndex = 0;
            else if (currentIndex >= list.Count) currentIndex = list.Count - 1;
            return true;
        }
        public List<Song> GetSongs()        //метод для получения списка песен
        {
            return list;
        }
    }
}
