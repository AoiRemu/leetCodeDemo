using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int x) { val = x; }
    }
    public class LeetCodeDemo
    {
        /// <summary>
        /// 二叉树的层序遍历
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IList<IList<int>> LevelOrder(TreeNode root)
        {
            List<IList<int>> list = new List<IList<int>>();
            if (root == null) { return list; }
            Queue<TreeNode> que = new Queue<TreeNode>();
            list.Add(new List<int>() { root.val });
            que.Enqueue(root);
            while (que.Count != 0)
            {
                int count = que.Count;
                List<int> tempList = new List<int>();
                for(var i = 0; i < count; i++)
                {
                    TreeNode tempNode = que.Dequeue();
                    if (tempNode.left != null)
                    {
                        que.Enqueue(tempNode.left);
                        tempList.Add(tempNode.left.val);
                    }
                    if (tempNode.right != null)
                    {
                        que.Enqueue(tempNode.right);
                        tempList.Add(tempNode.right.val);
                    }
                }
                if (tempList.Count != 0)
                {
                    list.Add(tempList);
                }
            }
            return list;
        }
        /// <summary>
        /// 对称二叉树
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        /// 思路：使用中序遍历，保存节点值，获取以根节点为中间点的数组，判断前半段数组和后半段数组是否对称
        public bool IsSymmetric(TreeNode root)
        {
            if (root == null) { return true; }
            return CheckSymmetric(root.left, root.right);
        }
        public bool CheckSymmetric(TreeNode left, TreeNode right)
        {
            if(left==null && right == null)
            {
                return true;
            }
            if(left==null || right == null)
            {
                return false;
            }
            if (left.val != right.val)
            {
                return false;
            }
            return CheckSymmetric(left.left, right.right) && CheckSymmetric(left.right, right.left);
        }
        /// <summary>
        /// 验证二叉搜索树
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public bool IsValidBST(TreeNode root)
        {
            return CheckValidBST(root, long.MaxValue, long.MinValue);
        }
        public bool CheckValidBST(TreeNode root,long top,long down)
        {
            if (root == null) { return true; }
            if(root.val >= top || root.val <= down)
            {
                return false;
            }
            return CheckValidBST(root.left, root.val, down) && CheckValidBST(root.right, top, root.val);
        }
        /// <summary>
        /// 二叉树的最大深度
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int MaxDepth(TreeNode root)
        {
            //if (root == null) { return 0; }
            //int leftDep = MaxDepth(root.left);
            //int rightDep = MaxDepth(root.right);
            //return Math.Max(leftDep, rightDep) + 1;
            if (root == null) { return 0; }
            Queue<TreeNode> queue = new Queue<TreeNode>();
            int dep = 0;
            queue.Enqueue(root);
            //遍历二叉树的每一层，在进入下一层时深度+1
            while (queue.Count != 0)
            {
                dep++;
                int count = queue.Count;
                for(var i=0;i< count; i++)
                {
                    TreeNode temp = queue.Dequeue();//取出队列的第一个元素
                    //将下一层的节点加入队列
                    if (temp.left != null)
                    {
                        queue.Enqueue(temp.left);//加入队列
                    }
                    if (temp.right != null)
                    {
                        queue.Enqueue(temp.right);
                    }
                }
            }
            return dep;
        }
        /// <summary>
        /// 环形链表
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public bool HasCycle(ListNode head)
        {
            if(head==null || head.next == null) { return false; }
            List<ListNode> list = new List<ListNode>();
            list.Add(head);
            ListNode node = head.next;
            while (node != null)
            {
                if (list.Contains(node))
                {
                    return true;
                }
                list.Add(node);
                node = node.next;
            }
            return false;
        }
        /// <summary>
        /// 回文链表
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public bool IsPalindrome(ListNode head)
        {
            if (head == null || head.next==null) { return true; }
            List<int> list = new List<int>();
            list.Add(head.val);
            ListNode node = head.next;
            while (node != null)
            {
                list.Add(node.val);
                node = node.next;
            }
            for(var i = 0; i < list.Count / 2; i++)
            {
                if (list[i] != list[list.Count - i - 1])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 合并两个有序链表
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            if(l1==null && l2 == null) { return null; }
            List<int> numList = new List<int>();
            if (l1 != null)
            {
                numList.Add(l1.val);
                ListNode node = l1;
                while (node.next != null)
                {
                    numList.Add(node.next.val);
                    node = node.next;
                }
            }
            if (l2 != null)
            {
                numList.Add(l2.val);
                ListNode node = l2;
                while (node.next != null)
                {
                    numList.Add(node.next.val);
                    node = node.next;
                }
            }
            numList.Sort();
            ListNode head = new ListNode(numList[0]);
            if (numList.Count > 1)
            {
                ListNode nodeTemp = head;
                for (var i = 1; i < numList.Count; i++)
                {
                    nodeTemp.next = new ListNode(numList[i]);
                    nodeTemp = nodeTemp.next;
                }
            }
            return head;
        }
        /// <summary>
        /// 反转链表
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        public ListNode ReverseList(ListNode head)
        {
            ListNode preNode = null;
            ListNode node = head;
            while (node != null)
            {
                ListNode temp = node.next;
                node.next = preNode;
                preNode = node;
                node = temp;
            }
            return preNode;
        }
        /// <summary>
        /// 杨辉三角 II
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public IList<int> GetRow(int rowIndex)
        {
            List<int> nowList = new List<int>();
            for(var i = 0; i <= rowIndex+1; i++)
            {
                List<int> tempList = new List<int>();
                for(var j = 0; j < i; j++)
                {
                    if (j == 0 || j==i-1)
                    {
                        tempList.Add(1);
                    }
                    else
                    {
                        tempList.Add(nowList[j] + nowList[j - 1]);
                    }
                }
                nowList = tempList;
            }
            return nowList;
        }
        /// <summary>
        /// 长度最小的子数组
        /// </summary>
        /// <param name="s"></param>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int MinSubArrayLen(int s, int[] nums)
        {
            if (nums.Length == 0)
            {
                return 0;
            }
            int left = 0;
            int right = 0;
            int sum = 0;
            int count = int.MaxValue;
            while (right < nums.Length)
            {
                if (right == 0)
                {
                    sum = nums[right];
                }
                else
                {
                    sum += nums[right];
                }
                while (sum >= s)
                {
                   
                    int countTemp = right - left + 1;
                    if (countTemp < count)
                    {
                        count = countTemp;
                    }
                    sum -= nums[left];
                    left++;
                }
                right++;
            }
            return count==int.MaxValue?0:count;
        }
        /// <summary>
        /// 最大连续1的个数
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int FindMaxConsecutiveOnes(int[] nums)
        {
            int count = 0;
            int index = 0;
            for(int i = 0; i < nums.Length; i++)
            {
                if (nums[i] == 1)
                {
                    index++;
                }
                else
                {
                    if (count < index)
                    {
                        count = index;
                    }
                    index = 0;
                }
            }
            if (count < index)
            {
                count = index;
            }
            return count;
        }
        /// <summary>
        /// 移除元素  原地 移除
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public int RemoveElement(int[] nums, int val)
        {
            int length = 0;
            for(int i = 0; i < nums.Length; i++)
            {
                if (nums[i] != val)
                {
                    nums[length] = nums[i];
                    length++;
                }
            }
            return length;
        }
        /// <summary>
        /// 两数之和 II - 输入有序数组
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public int[] TwoSum2(int[] numbers, int target)
        {
            int[] result = new int[] { 0, 0};
            int left = 0;
            int right = numbers.Length - 1;
            while (left < right)
            {
                int sum = numbers[left] + numbers[right];
                if (sum == target)
                {
                    result[0] = left + 1;
                    result[1] = right + 1;
                    break;
                }
                else if (sum < target)
                {
                    left++;
                }
                else
                {
                    right--;
                }
            }
            return result;
        }
        /// <summary>
        /// 数组拆分 I
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int ArrayPairSum(int[] nums)
        {
            int sum = 0;
            Array.Sort(nums);
            for(var i = 0; i < nums.Length; i+=2)
            {
                sum += nums[i];
            }
            return sum;
        }

        /// <summary>
        /// 二进制求和
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public string AddBinary(string a, string b)
        {
            //反转
            var alist = a.ToList();
            var blist = b.ToList();
            alist.Reverse();
            blist.Reverse();
            int length = a.Length > b.Length ? a.Length : b.Length;
            int an = 0;//进位数
            List<int> list = new List<int>();
            for(var i = 0; i < length; i++)
            {
                int temp = 0;
                if(i>=alist.Count && i  < blist.Count)
                {
                    temp = an + Convert.ToInt32(blist[i].ToString());
                }else if(i< alist.Count && i >= blist.Count)
                {
                    temp= an + Convert.ToInt32(alist[i].ToString());
                }
                else
                {
                    temp = an + Convert.ToInt32(alist[i].ToString()) + Convert.ToInt32(blist[i].ToString());
                }
                if (temp == 2)
                {
                    an = 1;
                    temp = 0;
                }
                else if (temp == 3)
                {
                    an = 1;
                    temp = 1;
                }
                else
                {
                    an = 0;
                }
                list.Add(temp);
            }
            if (an == 1)
            {
                list.Add(1);
            }
            list.Reverse();
            string result = string.Join("", list);
            return result;
        }
        /// <summary>
        /// 445. 两数相加 II
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        /// <returns></returns>
        public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            List<int> list1 = new List<int>() { l1.val};
            List<int> list2 = new List<int>() { l2.val};
            //获取链表的值放入list中
            while (l1.next != null)
            {
                list1.Add(l1.next.val);
            }
            while (l2.next != null)
            {
                list2.Add(l1.next.val);
            }
            //反转为反序
            list1.Reverse();
            list2.Reverse();
            //获取最大长度
            int length = list1.Count >= list2.Count ? list1.Count : list2.Count;
            List<int> resultList = new List<int>();
            int an = 0;//进位数
            for(var i = 0; i < length; i++)
            {
                int temp = 0;
                if (i >= list1.Count && i<list2.Count)
                {
                    temp = list2[i] + an;
                }
                else if(i>=list2.Count && i < list1.Count)
                {
                    temp = list1[1] + an;
                }
                else
                {
                    temp = list1[i] + list2[i] + an;
                }
                resultList.Add(temp % 10);
                //大于10进位1
                an = temp >= 10 ? 1 : 0;
            }
            //反转为正序
            resultList.Reverse();
            ListNode node = new ListNode(resultList[0]);
            for(var i = 1; i < resultList.Count; i++)
            {
                node.next = new ListNode(resultList[i]);
                node = node.next;
            }
            return null;
        }
        /// <summary>
        /// 杨辉三角
        /// </summary>
        /// <param name="numRows"></param>
        /// <returns></returns>
        public IList<IList<int>> Generate(int numRows)
        {
            List<IList<int>> list = new List<IList<int>>();
            for(var i = 1; i <= numRows; i++)
            {
                List<int> tempList = new List<int>() { };
                for(var j = 0; j < i; j++)
                {
                    if (list.Count == 0)
                    {
                        tempList.Add(1);
                    }
                    else
                    {
                        if (j == 0 || j == i - 1)
                        {
                            tempList.Add(1);
                        }else
                        {
                            int num = list[i - 2][j - 1] + list[i - 2][j];
                            tempList.Add(num);
                        }
                    }
                }
                list.Add(tempList);
            }
            return list;
        }
        /// <summary>
        /// 151. 翻转字符串里的单词
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ReverseWords(string s)
        {
            if(string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s))
            {
                return "";
            }
            string[] arr = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Array.Reverse(arr);
            return string.Join(" ", arr);
        }
        /// <summary>
        ///  反转字符串中的单词 III
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ReverseWords3(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s))
            {
                return "";
            }
            string[] arr = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for(var i = 0;i<arr.Length;i++)
            {
                var arrTemp = arr[i].ToArray();
                Array.Reverse(arrTemp);
                arr[i] = new string(arrTemp);
            }
            return string.Join(" ", arr);
        }


        /// <summary>
        ///  螺旋矩阵
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public IList<int> SpiralOrder(int[][] matrix)
        {
            List<int> ans = new List<int>();
            if (matrix == null)
            {
                return ans;
            }
            if(matrix.Length==0)
            {
                return ans;
            }
            int R = matrix.Length;
            int C = matrix[0].Length;
            bool[,] seen = new bool[R, C];//标记是否访问过

            //四个标量分别代表 右 下 左 上 读取的坐标改变值
            //如 当前坐标(0,0) 下一个坐标为 (0+0,0+1)=(0,1)
            int[] dr = { 0, 1, 0, -1 };
            int[] dc = { 1, 0, -1, 0 };
            //当前所在位置为 (r, c)，前进方向是 di
            int r = 0, c = 0, di = 0;
            //遍历所有单元格
            for(var i = 0; i < R * C; i++)
            {
                ans.Add(matrix[r][c]);
                seen[r,c] = true;//标记为已访问
                int cr = r + dr[di];//下一个r的位置
                int cc = c + dc[di];//下一个c的位置
                //下一个位置坐标合法
                if(0<=cr && cr < R && 0 <= cc && cc < C && !seen[cr, cc])
                {
                    r = cr;
                    c = cc;
                }
                else
                {
                    di = (di + 1) % 4;//方向改变
                    r += dr[di];
                    c += dc[di];
                }
            }
            return ans;
        }
        //对角线遍历
        public static int[] FindDiagonalOrder(int[][] matrix)
        {
            if (matrix == null || matrix.Length == 0 || matrix[0].Length == 0) { return new int[] { }; };
            //m行 n列
            int m = matrix.Length;
            int n = matrix[0].Length;
            List<int> numList = new List<int>();
            for (int i = 0; i < m + n - 1; i++)
            {
                if (i % 2 == 0)
                {
                    int x = i < m ? i : m - 1;
                    int y = i - x;
                    //开始遍历 x坐标不断减 y坐标不断加 当x减到0 或者 y加到列n的值
                    while (x >= 0 && y < n)
                    {
                        numList.Add(matrix[x][y]);
                        x--;
                        y++;
                    }

                }
                else
                {
                    //沿对角线向下遍历
                    int y = i < n ? i : n - 1;
                    int x = i - y;
                    //y坐标不断减 x坐标不断加 当y减到0 或者 x加到行m的值
                    while (y >= 0 && x < m)
                    {
                        numList.Add(matrix[x][y]);
                        y--;
                        x++;
                    }
                }
            }
            return numList.ToArray();
        }
        //至少是其他数字两倍的最大数
        public static int DominantIndex(int[] nums)
        {
            if (nums.Length == 0 || nums == null)
            {
                return -1;
            }
            int max = 0;
            int index = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                if (max < nums[i])
                {
                    max = nums[i];
                    index = i;
                }
            }
            for (int i = 0; i < nums.Length; i++)
            {
                if (max < nums[i] * 2 && i != index)
                {
                    return -1;
                }
            }
            return index;
        }
        //寻找数组的中心索引
        public static int PivotIndex(int[] nums)
        {
            int sumLeft = 0;
            int sumRight = 0;
            int sum = nums.Sum();
            for (int i = 0; i < nums.Length; i++)
            {
                if (sumLeft == sum - sumLeft - nums[i])
                {
                    return i;
                }
                sumLeft += nums[i];

            }
            return -1;
        }
        //力扣 删除链表的倒数第N个节点
        public ListNode RemoveNthFromEnd(ListNode head, int n)
        {
            ListNode temp = new ListNode(0);
            temp.next = head;
            ListNode first = temp;
            ListNode sec = temp;
            for (int i = 0; i <= n; i++)
            {
                first = first.next;
            }
            while (first != null)
            {
                first = first.next;
                sec = sec.next;
            }
            sec.next = sec.next.next;
            return temp.next;
        }

        public void ShowTest1()
        {
            Console.WriteLine("添加事件的普通方法");
        }
        public static string LongestCommonPrefix(string[] strs)
        {
            if (strs.Length == 0)
            {
                return "";
            }

            string temp = "";
            bool an = true;
            for (int i = 0; i < strs[0].Length; i++)
            {
                temp += strs[0][i];
                for (int j = 1; j < strs.Length; j++)
                {
                    if (strs[j].IndexOf(temp) != 0)
                    {
                        an = false;
                        break;
                    }
                }
                if (an == false)
                {
                    break;
                }
            }
            if (temp == "")
            {
                return "";
            }
            if (an == false)
            {
                temp = temp.Substring(0, temp.Length - 1);
            }
            return temp;
        }
        public static string CountAndSay(int n)
        {
            var temp = "1";
            int num = 0;
            while (num < n - 1)
            {
                temp = loop(temp);
                num++;
            }
            return temp;
            string loop(string s)
            {
                if (s == "1")
                {
                    return "11";
                }
                int count = 0;//字符出现次数
                string str = "";
                var item = s[0];
                for (int i = 0; i < s.Length; i++)
                {
                    if (item == s[i])
                    {
                        count++;
                    }
                    else
                    {
                        str = str + count + "" + item;
                        item = s[i];
                        count = 1;
                    }
                    if (i == s.Length - 1)
                    {
                        str = str + count + "" + item;
                    }
                }
                return str;
            }
        }
        public static int StrStr(string haystack, string needle)
        {
            int length = haystack.Length;
            if (string.IsNullOrEmpty(needle)) return 0;
            int neIndex = 0;
            int result = -1;
            for (int i = 0; i < length; i++)
            {
                if (length - i < needle.Length) return -1;
                string temp = haystack.Substring(i, needle.Length);
                if (needle == temp)
                {
                    return i;
                }
            }
            return -1;
        }
        public static int MyAtoi(string str)
        {
            str = str.Trim();
            if (string.IsNullOrEmpty(str)) return 0;
            string temp = "";
            int x = 0;
            try
            {
                if (str[0] == '-')
                {
                    foreach (var item in str.Substring(1))
                    {
                        if (char.IsNumber(item))
                        {
                            temp += item;
                        }
                        else
                        {
                            break;
                        }
                    }
                    x = string.IsNullOrEmpty(temp) ? 0 : Convert.ToInt32('-' + temp);
                    return x;
                }
                if (str[0] == '+')
                {
                    str = str.Substring(1);
                }
                foreach (var item in str)
                {
                    if (char.IsNumber(item))
                    {
                        temp += item;
                    }
                    else
                    {
                        break;
                    }
                }
                x = string.IsNullOrEmpty(temp) ? 0 : Convert.ToInt32(temp);

            }
            catch
            {
                if (str[0] == '-')
                {
                    return int.MinValue;
                }
                else
                {
                    return int.MaxValue;
                }
            }

            return x;
        }
        public static bool IsPalindrome(string s)
        {
            s = s.ToLower();
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }
            int i = 0, j = s.Length - 1;
            while (i < j)
            {
                char n = s[i];
                char m = s[j];
                if (!char.IsLetterOrDigit(s[i]) || char.IsWhiteSpace(s[i]))
                {
                    i++;
                    continue;
                }
                if (!char.IsLetterOrDigit(s[j]) || char.IsWhiteSpace(s[j]))
                {
                    j--;
                    continue;
                }
                if (s[i] != s[j])
                {
                    return false;
                }
                i++;
                j--;
            }
            return true;
        }
        public static bool IsAnagram(string s, string t)
        {
            Dictionary<char, int> list_s = new Dictionary<char, int>();
            Dictionary<char, int> list_t = new Dictionary<char, int>();
            foreach (char item in s)
            {
                if (list_s.ContainsKey(item))
                {
                    list_s[item]++;
                }
                else
                {
                    list_s.Add(item, 1);
                }
            }
            foreach (char item in t)
            {
                if (list_t.ContainsKey(item))
                {
                    list_t[item]++;
                }
                else
                {
                    list_t.Add(item, 1);
                }
            }
            if (list_s.Keys.Count != list_t.Count)
            {
                return false;
            }
            try
            {
                foreach (char item in list_s.Keys)
                {
                    if (list_s[item] != list_t[item])
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public static int FirstUniqChar(string s)
        {
            int[] co = new int['z' - 'a' + 1];
            foreach (var c in s)
            {
                co[c - 'a']++;
            }
            int min = int.MaxValue;
            for (int i = 0; i < co.Length; i++)
            {
                if (co[i] == 1)
                {
                    min = Math.Min(min, s.IndexOf((char)('a' + i)));
                }
            }

            return min == Int32.MaxValue ? -1 : min;
        }
        public static int Reverse(int x)
        {
            try
            {
                string str = x.ToString();
                if (str.IndexOf('-') > -1)
                {
                    str = str.Substring(1);
                    str = new string(str.Reverse().ToArray());
                    return Convert.ToInt32('-' + str);
                }
                str = new string(str.Reverse().ToArray());

                x = Convert.ToInt32(str);
            }
            catch
            {
                x = 0;
            }
            return x;
        }
        public static void ReverseString(char[] s)
        {
            for (int i = 0; i < s.Length / 2; i++)
            {
                var temp = s[i];
                s[i] = s[s.Length - 1 - i];
                s[s.Length - 1 - i] = temp;
            }
        }
        public static void Rotate(int[][] matrix)
        {
            int length = matrix.Length;
            for (int i = 0; i < length; i++)
            {
                for (int j = i; j < length; j++)
                {
                    int temp = matrix[j][i];
                    matrix[j][i] = matrix[i][j];
                    matrix[i][j] = temp;
                }
            }
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length / 2; j++)
                {
                    int tmp = matrix[i][j];
                    matrix[i][j] = matrix[i][length - j - 1];
                    matrix[i][length - j - 1] = tmp;
                }
            }
        }
        public static bool IsValidSudoku(char[][] board)
        {
            List<List<char>> hang = new List<List<char>>() {
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
            };
            List<List<char>> gong = new List<List<char>>() {
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
                new List<char>(){ },
            };
            for (int i = 0; i < board.Length; i++)
            {
                List<char> lie = new List<char>();
                for (int j = 0; j < board[i].Length; j++)
                {
                    if (board[i][j] != '.')
                    {
                        //列
                        if (lie.Contains(board[i][j]))
                        {
                            return false;
                        }
                        lie.Add(board[i][j]);
                        hang[j].Add(board[i][j]);
                        int gongI = i / 3 + j / 3;
                        if (i >= 6)
                        {
                            gongI += 4;
                        }
                        else if (i >= 3)
                        {
                            gongI += 2;
                        }
                        if (gongI == 6)
                        {
                            char c = board[i][j];
                        }
                        gong[gongI].Add(board[i][j]);
                        //if(hang[j].Contains(board[i][j]) || gong[gongI].Contains(board[i][j]))
                        //{
                        //    return false;
                        //}
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (Method(hang[i]))
                {
                    return false;
                }
                if (Method(gong[i]))
                {
                    return false;
                }
            }
            bool Method(List<char> list)
            {
                HashSet<char> temp = new HashSet<char>();
                foreach (char item in list)
                {
                    temp.Add(item);
                }
                return temp.Count != list.Count;
            }
            return true;
        }
        public static int[] TwoSum(int[] nums, int target)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (dic.ContainsKey(target - nums[i]))
                {
                    return new int[] { dic[target - nums[i]], i };
                }
                if (dic.ContainsKey(nums[i]))
                {
                    dic[nums[i]] = i;
                }
                else
                {
                    dic.Add(nums[i], i);
                }
            }
            return null;
        }
        public static void MoveZeroes(int[] nums)
        {
            //快慢双指针法
            int n = nums.Length;
            int index = 0;
            for (int i = index; i < n; i++)
            {
                if (nums[i] != 0)
                {
                    //后一位下标前移
                    int temp = nums[i];
                    nums[index] = nums[i];
                    nums[i] = temp;
                    index++;
                }
            }
            //for (int i = 0; i < nums.Length; i++)
            //{
            //    if (nums[i] == 0 && i!= nums.Length-1)
            //    {

            //    }
            //}
        }
        private static void MoveZeroes2(int[] nums)
        {
            //快慢双指针法
            int n = nums.Length;
            int index = 0;
            for (int i = index; i < n; i++)
            {
                if (nums[i] != 0)
                {
                    int temp = nums[i];
                    nums[i] = nums[index];
                    nums[index] = temp;
                    index++;
                }
            }
        }

        private static void Swap(ref int num1, ref int num2)
        {
            int swap = num1;
            num1 = num2;
            num2 = swap;
        }


        public static int[] PlusOne(int[] digits)
        {
            int length = digits.Length;
            if (length == 0)
            {
                return new int[] { 1 };
            }
            List<int> num = digits.ToList();

            for (int i = length - 1; i >= 0; i--)
            {
                num[i] += 1;
                if (num[i] == 10)
                {
                    num[i] = 0;
                    if (i == 0)
                    {
                        num = new List<int>() { 1 }.Concat(num).ToList();
                    }
                }
                else
                {
                    break;
                }
            }
            return num.ToArray();
        }
    }
}
