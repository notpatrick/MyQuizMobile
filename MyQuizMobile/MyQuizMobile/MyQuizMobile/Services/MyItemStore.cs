using MyQuizMobile.Interfaces;
using MyQuizMobile.Model;
using MyQuizMobile.Services.Standard;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyItemStore))]
namespace MyQuizMobile.Services.Standard
{
    public class MyItemStore : BaseStore<MyItem>, IMyItemStore
    {
        public override string Identifier => "MyItem";
    }
}
