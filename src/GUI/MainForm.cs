using Draw.src.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Right)
            {
                var sel1 = dialogProcessor.ContainsPoint(e.Location);

                if (sel1 != null)
                {
                    
                    if (dialogProcessor.ShapeList.Contains(sel1))
                    {
                        contextMenuStrip1.Show(this, new Point(e.X, e.Y));
                       
                        dialogProcessor.SelectionMenu = sel1;
                      
                    }
                }
                else
                {
                    if (dialogProcessor.SelectionMenu != null)
                    {
                        contextMenuStrip2.Show(this, new Point(e.X, e.Y));
                        if (dialogProcessor.Coppy != null)
                        {
                            dialogProcessor.Coppy= new PointF(e.X, e.Y);
                        }
                    }
                }
            }
			if (pickUpSpeedButton.Checked) {
                var sel = dialogProcessor.ContainsPoint(e.Location);
			
				if (sel!= null) {
                    if (dialogProcessor.Selection.Contains(sel))
                        dialogProcessor.Selection.Remove(sel);
                    else
                        dialogProcessor.Selection.Add(sel);

                    statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
					dialogProcessor.IsDragging = true;
					dialogProcessor.LastLocation = e.Location;
					viewPort.Invalidate();
				}
			}
            
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dialogProcessor.IsDragging) {
				if (dialogProcessor.Selection != null) statusBar.Items[0].Text = "Последно действие: Влачене";
				dialogProcessor.TranslateTo(e.Location);
				viewPort.Invalidate();
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetSelectedFieldColor(colorDialog1.Color);
                viewPort.Invalidate();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomEllipsa();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTriangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";

            viewPort.Invalidate();
        }

        private void toolStripButtonGroup_Click(object sender, EventArgs e)
        {
            dialogProcessor.GroupSelected();
            statusBar.Items[0].Text = "Последно действие: Групиране";
            viewPort.Invalidate();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            dialogProcessor.ReGroup();
            statusBar.Items[0].Text = "Последно действие: Разгрупиране";
            viewPort.Invalidate();
        }
        
        private void button_resize_Click(object sender, EventArgs e)
        {
            float width;
            float height;
            try
            {
                if (!textBoxHeight.Text.Equals(""))
                {
                    height = float.Parse(textBoxHeight.Text);
                }
                else
                {
                    height = -1;
                }
                if (!textBoxWidth.Text.Equals(""))
                {
                    width = float.Parse(textBoxWidth.Text);
                }
                else
                {
                    width = -1;
                }
                dialogProcessor.SetSize(width, height);
            }
            catch(Exception)
            {
                MessageBox.Show("please insert valid number");
            }
            statusBar.Items[0].Text = "Последно действие: Преоразмеряване";
            viewPort.Invalidate();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

            dialogProcessor.DeleteSelected();
            statusBar.Items[0].Text = "Последно действие: изтриване на селектираните примитиви";
            viewPort.Invalidate();
        }
        

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.Selection.Remove(dialogProcessor.SelectionMenu);
            dialogProcessor.ShapeList.Remove(dialogProcessor.SelectionMenu);
            statusBar.Items[0].Text = "Последно действие: Изтриване на примитив";
            viewPort.Invalidate();
        }

        

        

        

        private void deleteSelectedItemToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dialogProcessor.DeleteSelected();
            statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните примитиви";
            viewPort.Invalidate();
        }

        private void deleteSelectedItemToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dialogProcessor.DeleteSelected();
            statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните примитиви";
            viewPort.Invalidate();
        }
        
        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            dialogProcessor.DeleteSelected();
            statusBar.Items[0].Text = "Последно действие: Изтриване на селектираните примитиви";
            viewPort.Invalidate();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomRectangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";

            viewPort.Invalidate();
        }

        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomEllipsa();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomTriangle();

            statusBar.Items[0].Text = "Последно действие: Рисуване на триъгълник";

            viewPort.Invalidate();
        }
        
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                dialogProcessor.SetSelectedBorderColor(colorDialog1.Color);
                statusBar.Items[0].Text = "Последно действие: Промяна на цвета на кунтура";
                viewPort.Invalidate();
            }
        }

        private void opacityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("opacity",dialogProcessor);
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Задаване на прозрачност";
            viewPort.Invalidate();
        }

        private void borderWidthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("border width", dialogProcessor);
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Задавене на дебелина на контура";
            viewPort.Invalidate();
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("rotate", dialogProcessor);
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Завъртане";
            viewPort.Invalidate();
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("resize", dialogProcessor);
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Преоразмеряване";
            viewPort.Invalidate();
        }

        private void opacityToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("opacity", dialogProcessor,"right");
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Задаване на прозрачност";
            viewPort.Invalidate();
        }

        private void borderWidthToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("border width", dialogProcessor,"right");
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Задаване дебелина на кънтура";
            viewPort.Invalidate();
        }

        private void resizeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("resize", dialogProcessor,"right");
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Преоразмеряване";
            viewPort.Invalidate();
        }

        private void rotateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Dialog eventForm = new Dialog("rotate", dialogProcessor, "right");
            eventForm.ShowDialog();
            statusBar.Items[0].Text = "Последно действие: Завъртане";
            viewPort.Invalidate();
        }

        private void okButton_Click(object sender, EventArgs e)
        {

        }

        private void mainMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                dialogProcessor.MySerialize(dialogProcessor.ShapeList, saveFileDialog1.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dialogProcessor.ShapeList = (List<Shape>)dialogProcessor.MyDeSerialize(openFileDialog1.FileName);
                viewPort.Invalidate();
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomLine();

            statusBar.Items[0].Text = "Последно действие: Рисуване на линия";

            viewPort.Invalidate();
        }
    }
}
