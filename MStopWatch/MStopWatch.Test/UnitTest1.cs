using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MStopWatch.VM;

// ViewModelを単体テストする
namespace MStopWatch.Test
{
    [TestClass]
    public class TestViewModel
    {
        [TestMethod]
        public void TestStart()
        {
            var vm = new StopWatchVM();
            vm.Reset();
            // 初期値を調べる
            Assert.AreEqual(0, vm.Mode);
            Assert.AreEqual("Start", vm.StartButtonText);
            Assert.AreEqual(new TimeSpan(0), vm.NowSpan);
            Assert.AreEqual(0, vm.Items.Count);
        }

        /// <summary>
        /// ラップを実行する
        /// </summary>
        [TestMethod]
        public void TestOneLap()
        {
            var vm = new StopWatchVM();
            vm.Start();
            Assert.AreEqual("Stop", vm.StartButtonText);
            System.Threading.Thread.Sleep(1000);

            vm.Lap();
            Assert.AreEqual("Stop", vm.StartButtonText);
            // ひとつだけ追加されている
            Assert.AreEqual(1, vm.Items.Count);

            System.Threading.Thread.Sleep(1000);
            vm.Stop();
            Assert.AreEqual("Restart", vm.StartButtonText);
        }

        /// <summary>
        /// ラップを実行する
        /// </summary>
        [TestMethod]
        public void TestLaps()
        {
            var vm = new StopWatchVM();
            vm.Start();
            Assert.AreEqual("Stop", vm.StartButtonText);
            System.Threading.Thread.Sleep(1000);

            vm.Lap();
            System.Threading.Thread.Sleep(100);
            vm.Lap();
            System.Threading.Thread.Sleep(100);
            vm.Lap();
            Assert.AreEqual("Stop", vm.StartButtonText);
            Assert.AreEqual(3, vm.Items.Count);

            System.Threading.Thread.Sleep(1000);
            vm.Stop();
            Assert.AreEqual("Restart", vm.StartButtonText);
        }

        /// <summary>
        /// リセットで元に戻る
        /// </summary>
        [TestMethod]
        public void TestReset()
        {
            var vm = new StopWatchVM();
            vm.Start();
            Assert.AreEqual("Stop", vm.StartButtonText);
            System.Threading.Thread.Sleep(1000);

            vm.Lap();
            Assert.AreEqual("Stop", vm.StartButtonText);
            // ひとつだけ追加されている
            Assert.AreEqual(1, vm.Items.Count);

            System.Threading.Thread.Sleep(1000);
            vm.Stop();
            Assert.AreEqual("Restart", vm.StartButtonText);

            vm.Reset();

            Assert.AreEqual("Start", vm.StartButtonText);
            Assert.AreEqual(new TimeSpan(0), vm.NowSpan);
            Assert.AreEqual(0, vm.Items.Count);
        }

    }
}
