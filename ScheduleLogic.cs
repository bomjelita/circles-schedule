using System;
using System.Collections.Generic;
using System.Linq;

namespace CirclesSchedule
{
    public static class ScheduleLogic
    {
        public static void AddCircle(string name, string leader, string days, string startTime, string endTime, string room, string ageGroup)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название кружка не может быть пустым");
            if (string.IsNullOrWhiteSpace(leader))
                throw new ArgumentException("ФИО руководителя не может быть пустым");

            var circles = DataManager.LoadData();

            // Проверка на дубликаты
            if (circles.Any(c => c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Кружок с таким названием уже существует в списке");
            }

            circles.Add(new CircleInfo
            {
                Name = name.Trim(),
                Leader = leader.Trim(),
                DaysOfWeek = days.Trim(),
                StartTime = startTime.Trim(),
                EndTime = endTime.Trim(),
                Room = room.Trim(),
                AgeGroup = ageGroup.Trim()
            });
            DataManager.SaveData(circles);
        }

        public static List<CircleInfo> GetAllCircles()
        {
            return DataManager.LoadData();
        }

        public static List<CircleInfo> SearchCircles(string query)
        {
            var circles = DataManager.LoadData();
            if (string.IsNullOrWhiteSpace(query))
                return circles;

            query = query.ToLower();
            return circles.Where(c =>
                c.Name.ToLower().Contains(query) ||
                c.Leader.ToLower().Contains(query) ||
                c.DaysOfWeek.ToLower().Contains(query)
            ).ToList();
        }

        public static void DeleteCircle(int index)
        {
            var circles = DataManager.LoadData();
            if (index >= 0 && index < circles.Count)
            {
                circles.RemoveAt(index);
                DataManager.SaveData(circles);
            }
        }

        public static void UpdateCircle(int index, CircleInfo updated)
        {
            var circles = DataManager.LoadData();
            if (index >= 0 && index < circles.Count)
            {
                // Проверка на дубликаты при редактировании (исключая сам редактируемый кружок)
                if (circles.Any((c, i) => i != index && c.Name.Equals(updated.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("Кружок с таким названием уже существует в списке");
                }

                circles[index] = updated;
                DataManager.SaveData(circles);
            }
        }
    }
}