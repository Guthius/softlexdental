/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using System;
using System.Collections;

namespace SparksToothChart
{
    /// <summary>
    /// A strongly typed collection of type <see cref="ToothGraphic"/>.
    /// </summary>
    public class ToothGraphicCollection : CollectionBase
    {
        /// <summary>
        /// Returns the ToothGraphic with the given index.
        /// </summary>
        public ToothGraphic this[int index]
        {
            get => (ToothGraphic)List[index];
            set
            {
                List[index] = value;
            }
        }

        /// <summary>
        /// Returns the ToothGraphic with the given toothID.
        /// </summary>
        public ToothGraphic this[string toothID]
        {
            get
            {
                if (toothID != "implant" && !ToothGraphic.IsValidToothID(toothID))
                {
                    throw new ArgumentException("Tooth ID not valid: " + toothID);
                }
                for (int i = 0; i < List.Count; i++)
                {
                    if (((ToothGraphic)List[i]).ToothID == toothID)
                    {
                        return (ToothGraphic)List[i];
                    }
                }
                return null;
            }
            set
            {
                //List[index]=value;
            }
        }

        public int Add(ToothGraphic value) => List.Add(value);

        public int IndexOf(ToothGraphic value) => List.IndexOf(value);

        public void Insert(int index, ToothGraphic value) => List.Insert(index, value);

        public void Remove(ToothGraphic value) => List.Remove(value);
        
        public bool Contains(ToothGraphic value) => List.Contains(value);
        
        protected override void OnInsert(int index, object value)
        {
            if (value.GetType() != typeof(ToothGraphic))
            {
                throw new ArgumentException("value must be of type ToothGraphic.", "value");
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (value.GetType() != typeof(ToothGraphic))
            {
                throw new ArgumentException("value must be of type ToothGraphic.", "value");
            }
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue.GetType() != typeof(ToothGraphic))
            {
                throw new ArgumentException("newValue must be of type ToothGraphic.", "newValue");
            }
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != typeof(ToothGraphic))
            {
                throw new ArgumentException("value must be of type ToothGraphic.");
            }
        }

        public ToothGraphicCollection Copy()
        {
            ToothGraphicCollection collect = new ToothGraphicCollection();
            for (int i = 0; i < Count; i++)
            {
                collect.Add(this[i].Copy());
            }
            return collect;
        }
    }
}
