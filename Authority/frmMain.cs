using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Authority
{
    public partial class frmMain : Form
    {

        private Authority authority = new  NormalAuthority();
        private System.Collections.ObjectModel.Collection<TreeView> trvAll=new System.Collections.ObjectModel.Collection<TreeView>();
        private MenuStrip mnuStrip ;
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(Authority au, MenuStrip mnu)
        {
            
            InitializeComponent();
            authority = au;
            mnuStrip = mnu;
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            //初始化用户选项
            cmbUser.DataSource = Enum.GetNames(typeof(Authority.User));


            for (int i = 0; i < cmbUser.Items.Count; i++)
            {
                TreeView trv = new TreeView();
                // trv.Name = "trvOperator";
                trv.Dock = DockStyle.Fill;
                trvAll.Add(trv);
                tbcAuthority.TabPages.Add(new TabPage(cmbUser.Items[i].ToString()));
                tbcAuthority.TabPages[i].Controls.Add(trv);

                trv.Nodes.Clear();
                foreach (var item in authority.Menus[cmbUser.Items[i].ToString()])
                {
                    trv.Nodes.Add(item.ToString());
                }
               

            }


           
            //foreach (var item in cmbUser.Items)
            //{
            //    TreeView trv = new TreeView();
            //    trv.Name = "trvOperator";

            //    trvAll.Add(trv);

            //    tbcAuthority.TabPages.Add(new TabPage(item.ToString))
            //}
            //trvAll.Clear();
            //trvAll.Add(trvOperator);
            //trvAll.Add(trvEngineer);
            //trvAll.Add(trvAdmin);

            //tbcAuthority.TabPages[0].Controls.Add(trvOperator);
            //for (int i = 0; i < length; i++)
            //{

            //}

           // this.Tag = menuStrip1;

            if (mnuStrip==null)
            {
                //MenuStrip mnuStrip = new MenuStrip();
                //mnuStrip =(MenuStrip)this.Tag;

                mnuStrip = menuStrip1;
               

            }
            //else
            //{

            //}
            for (int i = 0; i < mnuStrip.Items.Count; i++)
            {
                trvMenu.Nodes.Add(mnuStrip.Items[i].Text);
                TreeNodeCollection Nodes = trvMenu.Nodes[i].Nodes;
                AddItem((ToolStripMenuItem)mnuStrip.Items[i], Nodes);
            }


            // propertyGrid1.SelectedObject = authority;
        }
        private void AddItem(ToolStripMenuItem toolItems, TreeNodeCollection trnNodes = null)
        {
            for (int i = 0; i < toolItems.DropDownItems.Count; i++)
            {
                if (toolItems.DropDownItems.Count > 0)
                {
                    trnNodes.Add(toolItems.DropDownItems[i].Text);
                    TreeNodeCollection Nodes = trnNodes[i].Nodes;
                    AddItem((ToolStripMenuItem)toolItems.DropDownItems[i], Nodes);
                }
                else
                {
                    trnNodes.Add(toolItems.DropDownItems[i].Text);
                }

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (trvMenu .SelectedNode==null)
            {
                MessageBox.Show("请先选择项目");
                return;
            }
            else
            {

                // AddNode(trvMenu.SelectedNode);
                //if (trvMenu.SelectedNode.Parent)
                //{

                //}
                // MessageBox.Show(trvMenu.SelectedNode.Parent.Text);
                if (trvMenu.SelectedNode.Parent == null)
                {
                    //treeView1.Nodes.Add(trvMenu.SelectedNode.Text);
                    MessageBox.Show("请选择子项！");
                    return;
                }
                else
                {
                    //NodeNameStack = new Stack<string>();
                    //AddNode(trvMenu.SelectedNode);
                    string mnuName = trvMenu.SelectedNode.Text;
                    foreach (TreeNode item in trvAll[tbcAuthority.SelectedIndex].Nodes)
                    {
                        if (item.Text==mnuName)
                        {
                            MessageBox.Show("已经包含此项！");
                            return;
                        }
                    }
                    trvAll[tbcAuthority.SelectedIndex].Nodes.Add(mnuName);
                }
            }


        }


        //private void AddNode(TreeNode trvNode,ref TreeNode Node)
        //{



        //    if (trvNode.Parent!=null)
        //    {
        //       // AddNode(trvNode.Parent,);
        //    }
        //    else
        //    {
        //       // treeView1.Nodes.Add((TreeNode)trvNode.Clone());

        //    }
        //}
       // Stack<string> NodeNameStack;

        private void AddNode(TreeNode Node)
        {
            if (Node.Parent!=null)
            {
               // NodeNameStack.Push(Node.Text);
                AddNode(Node.Parent);
            }
            else
            {
                //treeView1.Nodes.Add()
            }


            //foreach (TreeNode item in Nodes)
            //{
            //    if (item.Text ==NodeName)
            //    {

            //    }
            //}


        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            

            if (trvAll[tbcAuthority.SelectedIndex].SelectedNode == null)
            {
                MessageBox.Show("请先选择要移除的菜单项目");
            }
            else
            {
                trvMenu.Nodes.Remove(trvAll[tbcAuthority.SelectedIndex].SelectedNode);

            }
        }

       

        private void btnOK_Click(object sender, EventArgs e)
        {

            txtNewPassword.Text = txtNewPassword.Text.Trim();
            txtConfirmPassword.Text = txtConfirmPassword.Text.Trim();

            if (txtNewPassword.Text == string.Empty)
            {
                MessageBox.Show("不允许空密码！");
                return;
            }


            string Psd = authority.VerifyPassword(txtNewPassword.Text.Trim(), txtConfirmPassword.Text.Trim());

            if (Psd==string.Empty)
            {
                MessageBox.Show("两次密码不一致或密码不符合要求，密码只能输入数字或字母");
                return;
            }

            txtNewPassword.Text = Psd;
            txtConfirmPassword.Text = Psd;

           

            //if (txtConfirmPassword.Text != txtNewPassword.Text)
            //{
            //    MessageBox.Show("两次密码不一致！");
            //    return;
            //}

            if (authority.ModifyPassword((Authority.User)cmbUser.SelectedIndex, Psd))
            {
                MessageBox.Show("密码修改成功！");
            }
            else
            {
                MessageBox.Show("密码修改失败！");
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cmbUser.Items.Count; i++)
            {
                authority.Menus[cmbUser.Items[i].ToString()].Clear();
                foreach (TreeNode item in trvAll[i].Nodes)
                {
                    authority.Menus[cmbUser.Items[i].ToString()].Add(item.Text);
                }

                //TreeView trv = new TreeView();
                //// trv.Name = "trvOperator";
                //trv.Dock = DockStyle.Fill;
                //trvAll.Add(trv);
                //tbcAuthority.TabPages.Add(new TabPage(cmbUser.Items[i].ToString()));
                //tbcAuthority.TabPages[i].Controls.Add(trv);

                //trv.Nodes.Clear();
                //foreach (var item in authority.Menus[cmbUser.Items[i].ToString()])
                //{
                //    trv.Nodes.Add(item.ToString());
                //}


            }

        }

        private void btnLoadDefault_Click(object sender, EventArgs e)
        {
            Authority.User usr = (Authority.User)cmbUser.SelectedIndex;
           if( MessageBox.Show("确认载入"+ usr.ToString() +"的初始密码","温馨提示", MessageBoxButtons.OKCancel)== DialogResult.Cancel)
            {
                return;
            }
            if (authority.LoadDefaultPassword(usr))
            {
                MessageBox.Show("成功载入" + usr.ToString() + "的初始密码");
            } 

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtNewPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
        }
    }
}
