using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using System.Windows.Automation;


namespace addressbook_tests_white
{
    public class GroupHelper : HelperBase
    {
        public static string GROUPWINTITLE = "Group editor";
        public static string DELETEGROUPWINTITLE = "Delete group";

        public GroupHelper(ApplicationManager manager) : base(manager) { }

        public List<GroupData> GetGroupList()
        {
            List<GroupData> list = new List<GroupData>();
            Window dialogue = OpenGroupsDialogue();
            Tree tree = dialogue.Get<Tree>("uxAddressTreeView");
            TreeNode root = tree.Nodes[0];
            foreach (TreeNode item in root.Nodes)
            {
                list.Add(new GroupData()
                {
                    Name = item.Text
                });
            }
            CloseGroupsDialogue(dialogue);
            return list;
        }

        public void Add(GroupData newGroup)
        {
            Window dialogue = OpenGroupsDialogue();
            dialogue.Get<Button>("uxNewAddressButton").Click();
            TextBox textBox = (TextBox)dialogue.Get(SearchCriteria.ByControlType(ControlType.Edit));
            textBox.Enter(newGroup.Name);
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            CloseGroupsDialogue(dialogue);
        }
        public void Remove(int index)
        {
            Window dialogue = OpenGroupsDialogue();

            Tree tree = dialogue.Get<Tree>("uxAddressTreeView");
            TreeNode root = tree.Nodes[0];
            root.Nodes[index].Select();
            Window deleteDialoogue = OpenDeleteGroupDialog(dialogue);
            SubmitGroupDelete(deleteDialoogue);

            CloseGroupsDialogue(dialogue);
        }

        private Window OpenGroupsDialogue()
        {
            manager.MainWindow.Get<Button>("groupButton").Click();
            return manager.MainWindow.ModalWindow(GROUPWINTITLE);
        }

        private void CloseGroupsDialogue(Window dialogue)
        {
            dialogue.Get<Button>("uxCloseAddressButton").Click();
        }
        public int GetGroupCount()
        {
            Window diaologue = OpenGroupsDialogue();
            Tree tree = diaologue.Get<Tree>("uxAddressTreeView");
            TreeNode root = tree.Nodes[0];
            int count = root.Nodes.Count();
            CloseGroupsDialogue(diaologue);
            return count;
        }
        public void CheckGroupExist()
        {
            if (GetGroupCount() <= 1)
            {
                GroupData newGroup = new GroupData()
                {
                    Name = "test"
                };
                Add(newGroup);
            }
        }
        private Window OpenDeleteGroupDialog(Window dialogue)
        {
            dialogue.Get<Button>("uxDeleteAddressButton").Click();
            return dialogue.ModalWindow(DELETEGROUPWINTITLE);
        }

        private void SubmitGroupDelete(Window dialogue)
        {
            dialogue.Get<Button>("uxOKAddressButton").Click();
        }
    }
}