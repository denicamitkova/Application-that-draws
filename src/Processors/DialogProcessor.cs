using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}

        #endregion

        #region Properties

        /// <summary>
        /// Избран елемент.
        /// </summary>
        private List<Shape> selection = new List<Shape>();
        public List<Shape> Selection
        {
            get { return selection; }
            set { selection = value; }
        }
       
        private Shape selectionMenu;
        public Shape SelectionMenu
        {
            get { return selectionMenu; }
            set { selectionMenu = value; }
        }

        private PointF coppy;
        public PointF Coppy
        {
            get { return coppy; }
            set { coppy = value; }
        }



        /// <summary>
        /// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
        /// </summary>
        private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}
		
		#endregion
		
		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
            
			rect.FillColor = Color.White;
            rect.BorderColor = Color.Black;
            rect.Opacity = 255;
            rect.BorderWidth = 1;

			ShapeList.Add(rect);
		}

        public void AddRandomEllipsa()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            ElipseShape elp = new ElipseShape(new Rectangle(x, y, 400, 200));
            elp.FillColor = Color.White;
            elp.BorderColor = Color.Black;
            elp.Opacity = 255;
            ShapeList.Add(elp);
        }

        public void AddRandomTriangle()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            TriangleShape tri = new TriangleShape(new Rectangle(x, y, 100, 200));
            tri.FillColor = Color.White;
            tri.BorderColor = Color.Black;
            tri.Opacity = 255;

            ShapeList.Add(tri);
        }
        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
        public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					//ShapeList[i].FillColor = Color.Red;
						
					return ShapeList[i];
				}	
			}
			return null;
		}

        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        public void TranslateTo(PointF p)
		{
            foreach (var item in Selection)
            {
                //item.Location = new PointF(item.Location.X + p.X - lastLocation.X, item.Location.Y + p.Y - lastLocation.Y);
                item.Move(p.X - lastLocation.X, p.Y - lastLocation.Y);
                
                lastLocation = p;
            }
            
        }

        internal void SetSelectedFieldColor(Color color)
        {
            foreach (var item in Selection)
          
            {
                item.GroupFillColor(color);
                item.FillColor = color;
            }
        }

        internal void SetSelectedBorderColor(Color color)
        {
            foreach (var item in Selection)

            {
                item.GroupBorderColor(color);
                item.BorderColor= color;
            }
        }

        internal void SetOpacity(int opacity, string btn = " ")
        {
            if (btn.Equals("right"))
            {
                SelectionMenu.GroupOpacity(opacity);
                SelectionMenu.Opacity = opacity;
            }
            else
            {
                foreach (var item in Selection)

                {
                    item.GroupOpacity(opacity);
                    item.Opacity = opacity;

                }
            }
        }

        internal void SetSize(float width,float height, string btn = " ")
        {
            if (btn.Equals("right"))
            {
                if (width != -1)
                {
                    if (SelectionMenu.GetType().Equals(typeof(GroupShape)))
                    {
                        SelectionMenu.GroupReSizeWidth(width);
                    }
                    else
                    {
                        SelectionMenu.Width = width;
                    }
                }
                if (height != -1)
                {
                    if (SelectionMenu.GetType().Equals(typeof(GroupShape)))
                    {
                        SelectionMenu.GroupReSizeHeight(height);
                    }
                    else
                    {
                        SelectionMenu.Height = height;
                    }
                }

            }
            else
            {
                foreach (var item in Selection)

                {
                    if (width != -1)
                    {
                        if (item.GetType().Equals(typeof(GroupShape)))
                        {
                            item.GroupReSizeWidth(width);
                        }
                        else
                        {
                            item.Width = width;
                        }
                    }
                    if (height != -1)
                    {
                        if (item.GetType().Equals(typeof(GroupShape)))
                        {
                            item.GroupReSizeHeight(height);
                        }
                        else
                        {
                            item.Height = height;
                        }
                    }
                }
            }
        }
        
        internal void SetBorderWidth(float borderWidth, string btn = " ")
        {
            if (btn.Equals("right"))
            {
                SelectionMenu.GroupBorderWidth(borderWidth);
                SelectionMenu.BorderWidth = borderWidth;
            }
            else {
                foreach (var item in Selection)

                {
                    item.GroupBorderWidth(borderWidth);
                    item.BorderWidth = borderWidth;

                }
            }
        }
        internal void DeleteSelected()
        {
            foreach (var item in Selection)
            {
                ShapeList.Remove(item);

            }
            Selection.Clear();
        }

        public override void Draw(Graphics grfx)
        {
            base.Draw(grfx);
            
            foreach (var item in Selection)
            {
                item.RotateShape(grfx);
                if(item.GetType().Equals(typeof(TriangleShape)))
                grfx.DrawRectangle(Pens.Black, item.Location.X - 3-(item.BorderWidth/2), item.Location.Y - 3-(item.BorderWidth*2), item.Width + 6+(item.BorderWidth), item.Height + 6+(float)(item.BorderWidth*2.5));
                else
                    grfx.DrawRectangle(Pens.Black, item.Location.X - 3 - (item.BorderWidth / 2), item.Location.Y - 3 - (item.BorderWidth /2), item.Width + 6 + (item.BorderWidth), item.Height + 6 + (item.BorderWidth));
                grfx.ResetTransform();
            }
        }

        public void GroupSelected()
        {
            if (Selection.Count < 2) return;

            float minX=float.PositiveInfinity;
            float minY = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;
            foreach (var item in Selection)
            {
                if (minX > item.Location.X)
                {
                    minX = item.Location.X;
                }
                if (minY > item.Location.Y)
                {
                    minY = item.Location.Y;
                }
                if (maxX < item.Location.X+item.Width)
                {
                    maxX = item.Location.X+item.Width;
                }
                if (maxY < item.Location.Y+item.Height)
                {
                    maxY = item.Location.Y+item.Height;
                }
            }
            var group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
            group.SubItem = Selection;
            foreach (var item in Selection)
            {
                ShapeList.Remove(item);
            }

            Selection = new List<Shape>();
            Selection.Add(group);
            ShapeList.Add(group);
            
        }
        public void ReGroup()
        {
            for (int i=0;i<Selection.Count; i++)
            {
                if (Selection[i].GetType().Equals(typeof(GroupShape)))
                {
                    GroupShape group = (GroupShape)Selection[i];
                    
                    ShapeList.AddRange(group.SubItem);
                    group.SubItem.Clear();
                    Selection[i] = null;
                    group = null;
                    ShapeList.Remove(Selection[i]);
                    Selection.Remove(Selection[i]);
                    

                }
            }
            GC.Collect();
        }
        //(написано в моя код)
        public void Rotate(float angle,string btn=" ")
        {
            if (btn.Equals("right"))
            {
                SelectionMenu.GroupRotate(angle);
                SelectionMenu.Angle = angle;
            }
            else
            {
                if (Selection.Count != 0)
                {
                    foreach (var item in Selection)
                    {
                        item.GroupRotate(angle);
                        item.Angle = angle;

                    }
                }
            }
        }


        public void MySerialize(object obj, string filePath = null)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream;
            if (filePath != null)
            {
                stream = new FileStream(filePath + ".asd",
                                  FileMode.Create);
            }
            else
            {
                stream = new FileStream("MyFile.asd",
                                        FileMode.Create,
                                        FileAccess.Write, FileShare.None);
            }
            formatter.Serialize(stream, obj);
            stream.Close();
        }



        public object MyDeSerialize(string filePath = null)
        {
            object obj;
            IFormatter formatter = new BinaryFormatter();
            Stream stream;
            if (filePath != null)
            {
                stream = new FileStream(filePath,
                                     FileMode.Open,
                                     FileAccess.Read, FileShare.None);
            }
            else
            {
                stream = new FileStream("MyFile.asd",
                                    FileMode.Open);
            }
            obj = formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

        public void AddRandomLine()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            LineShape letter = new LineShape(new Rectangle(x, y, 200, 200));
            letter.FillColor = Color.White;
            letter.BorderColor = Color.Black;
            letter.Opacity = 255;
            letter.BorderWidth = 1;

            ShapeList.Add(letter);
        }


    }
}
