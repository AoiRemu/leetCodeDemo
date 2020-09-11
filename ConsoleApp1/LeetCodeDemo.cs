using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Configuration;

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
        #region 每日一题
        /// <summary>
        /// 841. 钥匙和房间 2020/08/31
        /// </summary>
        /// <param name="rooms"></param>
        /// <returns></returns>
        public bool CanVisitAllRooms(IList<IList<int>> rooms)
        {
            /*
             * 题意：如果能进入所有房间则返回true，否则false。
             * 直观解法：从 0 号房间开始，找到钥匙进入对应房间继续获取钥匙，如果钥匙已经打开过房间或者没有房间可以打开，则回溯，使用下一把钥匙。
             * 使用深度优先搜索法，进行递归
             * 思路：使用dic<int,bool>作为房间是否使用的标记，构造dfs(int)方法，传入房间号，获取钥匙，若有钥匙，则dic标记为true，dfs(钥匙对应的房间号)，深度递归后，返回dic是否全部为true
             */
            //初始化房间使用标记
            //看过题解后，此处可以用数组来减少开销 bool[] dic = new bool[rooms.Count - 1];
            Dictionary<int, bool> dic = new Dictionary<int, bool>();
            for(var i = 0; i < rooms.Count; i++)
            {
                dic.Add(i, false);
            }
            dfs(0);
            foreach (var item in dic.Keys)
            {
                if (!dic[item])
                {
                    return false;
                }
            }
            return true;
            void dfs(int room)
            {
                if (!dic[room])
                {
                    dic[room] = true;
                    foreach (var item in rooms[room])
                    {
                        dfs(item);
                    }
                }
            }
        }
        /// <summary>
        /// 332. 重新安排行程 2020/08/27
        /// </summary>
        /// <param name="tickets"></param>
        /// <returns></returns>
        public IList<string> FindItinerary(IList<IList<string>> tickets)
        {
            /*
             * 题意为 使所有机票的起点与终点能有效，多条路径时，使用自然排序靠前的路径 即 字符相加的值更少的字符靠前
             * 使用dic,将起始点作为Key,终点作为value 即 dic<string,List<string>>
             * 直观感觉的做法：从机票的起点-终点，查找到上一机票终点为起点的机票， 重复此操作一直找到最终点，并且所有路径都被用过一遍，优先查找字符相加的值更少的
             */

            //想不出具体怎么实现，以下是官方题解Java版转化的代码
            Dictionary<string, Queue<string>> dic = new Dictionary<string, Queue<string>>();
            List<string> resultList = new List<string>();

            foreach (var ticket in tickets)
            {
                string src = ticket[0], dst = ticket[1];
                if (!dic.ContainsKey(src))
                {
                    dic.Add(src, new Queue<string>());
                }
                dic[src].Enqueue(dst);
            }
            //按字母排序
            foreach(var from in dic.Keys)
            {
                var tempList = new List<string>();
                while (dic[from].Count > 0)
                {
                    tempList.Add(dic[from].Dequeue());
                }
                tempList = tempList.OrderBy(a => a).ToList();
                tempList.ForEach(a =>
                {
                    dic[from].Enqueue(a);
                });
            }
            dfs("JFK");
            resultList.Reverse();
            return resultList;
            
            void dfs(string curr)
            {
                while(dic.ContainsKey(curr) && dic[curr].Count > 0)
                {
                    string tmp = dic[curr].Dequeue();
                    dfs(tmp);
                }
                resultList.Add(curr);
            }
        }

        /// <summary>
        /// 459. 重复的子字符串 2020/08/24
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool RepeatedSubstringPattern(string s)
        {
            //1.暴力解法 会超时
            //string temp = "";
            //for (int i = 0; i < s.Length; i++)
            //{
            //    string str = "";
            //    temp += s[i];
            //    if (s.Length % temp.Length == 0 && s.Length / temp.Length > 1)
            //    {
            //        for (int j = 0; j < s.Length/temp.Length; j++)
            //        {
            //            str += temp;
            //        }
            //        if (str == s)
            //        {
            //            return true;
            //        }
            //    }
            //}
            //return false;

            //2.题解的暴力解法
            //int n = s.Length;
            //for (int i = 1; i * 2 <= n; ++i)
            //{
            //    if (n % i == 0)
            //    {
            //        bool match = true;
            //        for (int j = i; j < n; ++j)
            //        {
            //            if (s[j] != s[j - i])
            //            {
            //                match = false;
            //                break;
            //            }
            //        }
            //        if (match)
            //        {
            //            return true;
            //        }
            //    }
            //}
            //return false;

            //3.题解的字符串匹配解法 即 字符串s加上自身，去掉头尾字符，仍包含字符串s的话，则为重复字符串
            return (s + s).IndexOf(s, 1) != s.Length;
            //4.提交记录中有dp，试试dp解法
        }

        #endregion

        #region 树

        /// <summary>
        /// 二叉树的最小深度
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int MinDepth(TreeNode root)
        {
            if (root == null) { return 0; }
            int left = MinDepth(root.left);
            int right = MinDepth(root.right);
            if(root.left==null && root.right == null)
            {
                return 1;
            }
            if(root.left==null || root.right == null)
            {
                return left + right + 1;
            }
            return Math.Min(left, right) + 1;
        }

        #endregion
        #region 动态规划
        /// <summary>
        /// 647. 回文子串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int CountSubstrings(string s)
        {
            /*
             dp[i]为i长度的字符串中有的回文子串数量
                在第i个长度时，回文子串数量只增加了i长度中的每一个字符与s[i]构成回文字符串的数量
                即，dp[i]=dp[i-1]+CountStr(i);
                初始值 dp[0]=1;

                看过题解后：这个思路tmd是暴力法！！！O(n³)的复杂度，写个毛！但是居然通过了就挺秃然的 = =
             */


            //int res = 1;
            //if (s.Length == 0)
            //{
            //    return 0;
            //}
            //if (s.Length == 1)
            //{
            //    return 1;
            //}
            //for (int i = 1; i < s.Length; i++)
            //{
            //    res += CountStr(i);
            //}
            //return res;
            //int CountStr(int num)
            //{
            //    int count = 0;
            //    for (int i = 0; i <= num; i++)
            //    {
            //        if (IsLoopStr(i, num))
            //        {
            //            count++;
            //        }
            //    }
            //    return count;
            //}
            //bool IsLoopStr(int start, int end)
            //{
            //    while (start <= end)
            //    {
            //        if (s[start++] != s[end--])
            //        {
            //            return false;
            //        }
            //    }
            //    return true;
            //}

            /*
             正确的动态规划方法：
                dp[i][j]表示：字符串i到j位置的子串是否为回文字符串
                            |true   if i=j                                      一个字符的时候
                            |false  if s[i]!=s[j]                               两端的字符不相等肯定不是回文字符串
                dp[i][j] =  |true   if i<j && s[i]==s[j] && i+1==j              两个字符的时候
                            |true   if i<j && s[i]==s[j] && dp[i+1][j-1]==true  两端字符相等时，剩下的子串也为回文字符串
                            
             */
            int res = 0;
            bool[,] dp = new bool[s.Length, s.Length];
            for (int j = 0; j < s.Length; j++)
            {
                for (int i = 0; i <= j; i++)
                {
                    //一个字符
                    if (i == j && s[i]==s[j])
                    {
                        dp[i, j] = true;
                        res++;
                    }
                    //两个字符时
                    else if ((i + 1) == j && s[i] == s[j])
                    {
                        dp[i, j] = true;
                        res++;
                    }
                    else if (i < j && s[i] == s[j] && dp[i + 1, j - 1])
                    {
                        dp[i, j] = true;
                        res++;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 213. 打家劫舍 II
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int RobⅡ(int[] nums)
        {
            /*
             * 第一个和最后一个不能同时相加，只能选择一个
             * 转化为两个单列，[0,n-1]或[1,n]
             */
            if (nums.Length == 0) { return 0; }
            if (nums.Length == 1) { return nums[0]; }
            int start = nums[0];
            int end = nums[nums.Length - 1];
            //不偷最后一个时
            nums[nums.Length - 1] = 0;
            int sum1 = Rob(nums);
            //不偷第一个时
            nums[nums.Length - 1] = end;
            nums[0] = 0;
            int sum2 = Rob(nums);
            return Math.Max(sum1, sum2);
        }

        /// <summary>
        /// 打家劫舍
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int Rob(int[] nums)
        {
            /*
             思考过程：
                1.确定dp[i]的状态：在nums[i]时至少相隔一个数字的数字的最大和
                2.探寻dp[i]和nums[i]之间的关系：
                    dp[0]=nums[0];
                    dp[1]=max(nums[0],nums[1]);
                    dp[2]=max(nums[0],nums[1],nums[0]+nums[2],nums[2])
                    dp[3]=max(nums[0],nums[1],nums[0]+nums[2],nums[2],nums[1]+nums[3],nums[0]+nums[3],nums[3])
                    dp[4]=max(nums[0],nums[1],nums[0]+nums[2],nums[2],nums[1]+nums[3],nums[0]+nums[3],nums[3],nums[0]+nums[4],nums[1]+nums[4],nums[2]+nums[4],nums[4],nums[0]+nums[2]+nums[4])
                    dp[5]=max(nums[0],nums[1],nums[0]+nums[2],nums[2],nums[1]+nums[3],nums[0]+nums[3],nums[3],nums[0]+nums[4],nums[1]+nums[4],nums[2]+nums[4],nums[4],
                              nums[0]+nums[5],nums[1]+nums[5],nums[2]+nums[5],nums[3]+nums[5],nums[0]+nums[2]+nums[5],nums[0]+nums[3]+nums[5],nums[1]+nums[3]+nums[5])

                    dp[i]=max(dp[i-1],nums[0]到nums[i]的与nums[i]相关的所有排列组合的最大值) -> 此最大值即为 dp[i-2]+nums[i]
                3.状态转移方程则为：dp[i]=max(dp[i-1],dp[i-2]+nums[i])
             */
            if (nums.Length == 0) { return 0; }
            int[] dp = new int[nums.Length];
            for (int i = 0; i < nums.Length; i++)
            {
                if (i == 0)
                {
                    dp[i] = nums[i];
                }else if (i == 1)
                {
                    dp[i] = Math.Max(nums[i], nums[i - 1]);
                }
                else
                {
                    dp[i] = Math.Max(dp[i - 1], dp[i - 2] + nums[i]);
                }

            }
            return dp[nums.Length - 1];
            
        }

        /// <summary>
        /// 最大子序和
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int MaxSubArray(int[] nums)
        {
            int res = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                nums[i] += Math.Max(nums[i - 1],0);//小于0的就不加上去
                res = Math.Max(nums[i], res);//比较结果值
            }
            return res;
        }

        /// <summary>
        /// 买卖股票的最佳时机
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public int MaxProfit(int[] prices)
        {
            //1.暴力破解 枚举出所有可能性，时间复杂度O(n²)
            //if (prices.Length < 2) { return 0; }
            //int res = 0;
            //for (int i = 0; i < prices.Length; i++)
            //{
            //    for (int j = i+1; j < prices.Length; j++)
            //    {
            //        res = Math.Max(res, prices[j] - prices[i]);
            //    }
            //}
            //return res;

            //2.保存一个最小值,轮询数组减去最小值，取其中最大值
            int min = int.MaxValue;
            int result = 0;
            for (int i = 0; i < prices.Length; i++)
            {
                if (prices[i] < min)
                {
                    min = prices[i];
                }
                else
                {
                    result = Math.Max(result, prices[i] - min);
                }
            }
            return result;
        }

        /// <summary>
        /// 爬楼梯
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int ClimbStairs(int n)
        {
            //本质是斐波那契数列

            //1.暴力递归破解 1000 早上10点算到14点40分还没算出来，我算你个锤子
            //if (n <= 2)
            //{
            //    return n;
            //}
            //return ClimbStairs(n - 1) + ClimbStairs(n - 2);


            //2.使用dp数组保存状态 100万 耗时7ms
            //if (n <= 2)
            //{
            //    return n;
            //}
            //int[] dp = new int[n];
            //dp[0] = 1;
            //dp[1] = 2;
            //for (int i = 2; i < n; i++)
            //{
            //    dp[i] = dp[i - 1] + dp[i - 2];
            //}
            //return dp[n - 1];

            //3.两个数字保存状态 100万 4ms
            int mpre = 0;
            int pre = 0;
            int cur = 0;
            for (int i = 1; i <= n; i++)
            {
                if (i <= 2)
                {
                    cur = i;

                }
                else
                {
                    cur = pre + mpre;
                }
                mpre = pre;
                pre = cur;
            }
            return cur;
        }

        #endregion
        /// <summary>
        /// 415. 字符串相加
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <returns></returns>
        public string AddStrings(string num1, string num2)
        {
            //1.最直观的解法
            //bool num1ismin = num1.Length < num2.Length;
            //int length = num1ismin ? num2.Length : num1.Length;
            //int cut = Math.Abs(num1.Length - num2.Length);
            //int add = 0;
            //List<int> sb = new List<int>();
            ////倒序相加，模拟数学运算
            //for (int i = length-1; i >=0; i--)
            //{
            //    int temp1 = 0;
            //    int temp2 = 0;
            //    if (num1ismin)
            //    {
            //        if (i - cut >= 0)
            //        {
            //            temp1 = Convert.ToInt32(num1[i - cut].ToString());
            //        }
            //        temp2 = Convert.ToInt32(num2[i].ToString());
            //    }
            //    else
            //    {
            //        if (i - cut >= 0)
            //        {
            //            temp2 = Convert.ToInt32(num2[i - cut].ToString());
            //        }
            //        temp1 = Convert.ToInt32(num1[i].ToString());
            //    }
            //    int sum = temp1 + temp2 + add;
            //    if (sum >= 10)
            //    {
            //        add = 1;
            //        sb.Add(sum - 10);
            //    }
            //    else
            //    {
            //        add = 0;
            //        sb.Add(sum);
            //    }
            //}
            //if (add == 1)
            //{
            //    sb.Add(1);
            //}
            //sb.Reverse();
            //string str = string.Join("",sb);
            //return str;

            //2.指针（md早该想到的）
            int n = num1.Length - 1;
            int m = num2.Length - 1;
            int add = 0;
            List<char> list = new List<char>();
            while(n>=0 || m >= 0)
            {
                int sum = add;
                if (n >= 0)
                {
                    sum += num1[n--] - '0';//char的数字字符减去 0 的char字符等于它本来的值
                }
                if (m >= 0)
                {
                    sum += num2[m--] - '0';
                }
                if (sum > 9)
                {
                    add = 1;
                    sum -= 10;
                }
                else
                {
                    add = 0;
                }
                list.Add((char)(sum + '0'));//再转换为char字符
            }
            if (add == 1)
            {
                list.Add('1');
            }
            list.Reverse();
            return new string(list.ToArray());
        }
        /// <summary>
        /// 41. 缺失的第一个正数
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int FirstMissingPositive(int[] nums)
        {
            //1.使用哈希表
            //HashSet<int> hash = new HashSet<int>();
            //foreach(int item in nums)
            //{
            //    if(item > 0)
            //    {
            //        hash.Add(item);
            //    }
            //}
            //int min = int.MaxValue;
            //if (hash.Count == 0) { return 1; }
            //for (int i = 1; i <= nums.Length+1; i++)
            //{
            //    if (!hash.Contains(i) && i<min)
            //    {
            //        min = i;
            //    }
            //}
            //return min;

            //2.对原数组进行处理，如果数字在[1-n]之间，变为负数，否则，将值变为n+1
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] < 1)
                {
                    nums[i] = nums.Length + 1;
                }
            }
            for (int i = 0; i < nums.Length; i++)
            {
                int num = Math.Abs(nums[i]);
                if (num <= nums.Length)
                {
                    if (nums[num - 1] > 0)
                    {
                        nums[num - 1] *= -1;
                    }
                }
            }
            //大于0的数的下标+1，就是最小的没有出现的正整数
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > 0)
                {
                    return i + 1;
                }
            }
            return nums.Length + 1;
        }
        /// <summary>
        /// 442. 数组中重复的数据
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public IList<int> FindDuplicates(int[] nums)
        {
            IList<int> list = new List<int>();
            for (int i = 0; i < nums.Length; i++)
            {
                int num = nums[i];
                if (nums[i] > nums.Length)
                {
                    num -= nums.Length;
                }
                int index = Math.Abs(num);
                nums[index - 1] *= -1;
                if(nums[index - 1] > 0)
                {
                    nums[index - 1] += nums.Length;
                }
            }
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > nums.Length)
                {
                    list.Add(i + 1);
                }
            }

            return list;
        }
        /// <summary>
        /// 448. 找到所有数组中消失的数字
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public IList<int> FindDisappearedNumbers(int[] nums)
        {
            IList<int> list = new List<int>();
            //将遍历到的数字作为下标，给访问到的数字反转为负数
            //如 nums[0]=2  则将 num[2-1]*=-1  
            //减一是因为数字范围为 1-n ,需要转化为下标
            for (int i = 0; i < nums.Length; i++)
            {
                int index = Math.Abs(nums[i]);
                if(nums[index - 1] > 0)
                {
                    nums[index - 1] *= -1;
                }
            }
            //可以这么做的原因是因为 数组大小是固定的n,数字范围是[1-n]，则数字作为下标来访问数组的话，必定能全部访问到的，没有访问到的就是消失的数字
            //如 [2,2,3,3] 的数组大小为4，数字范围是[1-4]，则 nums[2-1] *= -1 == -2,nums[3-1] *= -1 == -3,此时数组为 [2,-2,-3,3],没有访问到的数字下标为 0,3，即是[1,4]
            for(int i = 0; i < nums.Length; i++)
            {
                if (nums[i] > 0)
                {
                    list.Add(i + 1);
                }
            }

            return list;
        }
        /// <summary>
        /// 697. 数组的度
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int FindShortestSubArray(int[] nums)
        {
            //找出出现次数最多的数 key - 数字  value - 数组下标
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
            for (int i = 0; i < nums.Length; i++)
            {
                if (dic.ContainsKey(nums[i]))
                {
                    dic[nums[i]].Add(i);
                }
                else
                {
                    dic.Add(nums[i], new List<int>() { i });
                }
            }
            //可能有相同出现次数的数
            List<int> maxNums = new List<int>();
            int maxCount = 0;
            foreach(var item in dic.Keys)
            {
                if (dic[item].Count > maxCount)
                {
                    maxCount = dic[item].Count;
                    maxNums.Clear();
                    maxNums.Add(item);
                }else if (dic[item].Count == maxCount)
                {
                    maxNums.Add(item);
                }
            }
            //最多出现的数字的最小子数组长度
            int minSub = int.MaxValue;
            foreach(var item in maxNums)
            {
                int sub = dic[item].Last() - dic[item].First() + 1;
                if (sub < minSub)
                {
                    minSub = sub;
                }
            }
            return minSub;
        }
        /// <summary>
        /// 645. 错误的集合
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int[] FindErrorNums(int[] nums)
        {
            //1. 利用map的解法
            Dictionary<int, int> dic = new Dictionary<int, int>();
            foreach (var item in nums)
            {
                if (dic.ContainsKey(item))
                {
                    dic[item]++;
                }
                else
                {
                    dic.Add(item, 1);
                }
            }
            int missing = 0;
            int copy = 0;
            for (int i = 1; i <= nums.Length; i++)
            {
                if (missing != 0 && copy != 0)
                {
                    break;
                }
                if (!dic.ContainsKey(i))
                {
                    missing = i;
                }
                else if (dic[i] == 2)
                {
                    copy = i;
                }
            }
            return new int[] { copy, missing };


        }
        /// <summary>
        /// 628. 三个数的最大乘积
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int MaximumProduct(int[] nums)
        {
            int max1 = int.MinValue;
            int max2 = int.MinValue;
            int max3 = int.MinValue;

            int min1 = int.MaxValue;
            int min2 = int.MaxValue;
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] < 0)
                {
                    if (nums[i] < min1)
                    {
                        min2 = min1;
                        min1 = nums[i];
                    }else if (nums[i] < min2)
                    {
                        min2 = nums[i];
                    }
                }
                if (nums[i] > max1)
                {
                    max3 = max2;
                    max2 = max1;
                    max1 = nums[i];
                }
                else if (nums[i] > max2)
                {
                    max3 = max2;
                    max2 = nums[i];
                }
                else if (nums[i] > max3)
                {
                    max3 = nums[i];
                }

            }
            int result= max1 * max2 * max3;
            if (min1<0 && min2 < 0 && !(max1<0&&max2<0&&max3<0))
            {
                int minResult = max1 * min1 * min2;
                if (minResult > result)
                {
                    return minResult;
                }
            }
            return result;
        }
        /// <summary>
        /// 414. 第三大的数
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int ThirdMax(int[] nums)
        {
            long max1 = long.MinValue;
            long max2 = long.MinValue;
            long max3 = long.MinValue;
            for (int i = 0; i < nums.Length; i++)
            {
                if(nums[i]==max1 || nums[i]==max2 || nums[i] == max3)
                {
                    continue;
                }
                if (nums[i] > max1)
                {
                    max3 = max2;
                    max2 = max1;
                    max1 = nums[i];
                }
                else if (nums[i] > max2)
                {
                    max3 = max2;
                    max2 = nums[i];
                }
                else if (nums[i] > max3)
                {
                    max3 = nums[i];
                }
            }
            if (max3 == long.MinValue)
            {
                return (int)max1;
            }
            return (int)max3;
        }
        /// <summary>
        /// 495. 提莫攻击
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public int FindPoisonedDuration(int[] timeSeries, int duration)
        {
            if(timeSeries.Length==0 || timeSeries == null) { return 0; }
            if (timeSeries.Length == 1) { return duration; };
            int time = duration;
            for (int i = 1; i < timeSeries.Length; i++)
            {
                if (timeSeries[i] - timeSeries[i - 1] >= duration)
                {
                    time += duration;
                }
                else
                {
                    time += timeSeries[i] - timeSeries[i - 1];
                }
            }
            return time;
        }
        /// <summary>
        /// 第一个错误的版本
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int FirstBadVersion(int n)
        {
            int left = 0;
            int right = n;
            while (left<right)
            {
                int index = left + (right - left) / 2;
                if (IsBadVersion(index))
                {
                    right = index;
                }
                else
                {
                    left = index + 1;
                }
            }
            return left;
        }
        bool IsBadVersion(int version)
        {
            return true;
        }
        /// <summary>
        /// 合并两个有序数组
        /// </summary>
        /// <param name="nums1"></param>
        /// <param name="m"></param>
        /// <param name="nums2"></param>
        /// <param name="n"></param>
        public void Merge(int[] nums1, int m, int[] nums2, int n)
        {
            int len1 = m - 1;
            int len2 = n - 1;
            int len = m + n - 1;
            while (len1>=0 && len2 >= 0)
            {
                if (nums1[len1] > nums2[len2])
                {
                    nums1[len] = nums1[len1];
                    len1--;
                }
                else
                {
                    nums1[len] = nums2[len2];
                    len2--;
                }
                len--;
            }
            Array.Copy(nums2, 0, nums1, 0, len2 + 1);
        }
        /// <summary>
        /// 将有序数组转换为二叉搜索树
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public TreeNode SortedArrayToBST(int[] nums)
        {
            return SortedArrayToBSTMethod(nums, 0, nums.Length - 1);
        }
        private TreeNode SortedArrayToBSTMethod(int[] nums, int lo, int hi)
        {
            if (lo > hi)
            {
                return null;
            }
            // 以升序数组的中间元素作为根节点 root。
            int mid = (hi + lo) / 2;
            TreeNode root = new TreeNode(nums[mid]);
            // 递归的构建 root 的左子树与右子树。
            root.left = SortedArrayToBSTMethod(nums, lo, mid - 1);
            root.right = SortedArrayToBSTMethod(nums, mid + 1, hi);
            return root;
        }
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
