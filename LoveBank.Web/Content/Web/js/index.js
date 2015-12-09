	function runImg(){};
	runImg.prototype={
		bigbox:null,//最外层容器
		boxul:null,//子容器ul
		imglist:null,//子容器img
		numlist:null,//子容器countNum
		ochange:null,
		index:0,//当前显示项
		timer:null,//控制图片转变效果
		play:null,//控制自动播放
		imgurl:[],//存放图片
		count:0,//存放的个数
		playTime:0,
		bgcolor:[],
		$:function(obj){
			if(typeof(obj)=='string') {
				if(obj.indexOf('#')>=0){
					obj=obj.replace('#','');
					if (document.getElementById(obj)) {
					    return	document.getElementById(obj)
					} else{
						alert('没有容器'+obj);
						return null;
					};
				}else{
					return document.createElement(obj);
				};
			} else{
				return obj;
			};
		},
		//程序入口
		action:function(){
			this.autoplay();
			this.mouseoverout();
		},
		//重置函数
		info:function(id){
			this.count=this.imgurl.length;
			this.bigbox=this.$(id);
			for (var i = 0; i < 2; i++) {
				var ul=this.$('ul');
				// var a=this.$('a');
				for (var j = 1; j <= this.count; j++) {
					var li=this.$('li');
					li.innerHTML=i==0?this.imgurl[j-1]:'<a href="###" title="">　</a>';
					ul.appendChild(li);
				};
				this.bigbox.appendChild(ul);
				// this.bigbox.appendChild(a);
			};
			// this.achange=this.bigbox.getElementsByTagName('a');
			this.boxul=this.bigbox.getElementsByTagName('ul');
			this.imglist=this.boxul[0].getElementsByTagName('li');
			this.numlist=this.boxul[1].getElementsByTagName('li');

			// this.achange[0].className='aleft';
			// this.achange[1].className='aright';
			this.boxul[0].className='imgList';
			this.boxul[1].className='listnb';
			this.imglist[0].className="n1";
			this.numlist[0].className='active';

			this.action();
		},
		//图片切换效果，淡入
		imgshow:function(num){//此处参数为必须。
			var alpha=0;
			if (num<0) {num=this.count-1} else if(num>=this.count){num=0}; 
			this.index=num;
			
			var _this=this;

			for (var i = 0; i < this.count; i++) {
				this.numlist[i].className='';
				this.imglist[i].style.opacity=0;
				this.imglist[i].style.filter='alpha(opacity=0)';

			};
			this.numlist[this.index].className='active';
			clearInterval(this.timer);
			this.timer=setInterval(function(){

				alpha+=2;
				if (alpha>100) {alpha=100};
				_this.imglist[_this.index].style.opacity=alpha/100;
				_this.imglist[_this.index].style.filter='alpha(opacity='+alpha+')';
				// alert(alpha);
				if (alpha==100) {clearInterval(_this.timer)};
			},20);
		},
		//自动播放效果
		autoplay:function(){
			var _this=this;
			this.play=setInterval(function(){
				_this.index++;
				if (_this.index>_this.count-1) {_this.index=0};
				_this.imgshow(_this.index);
			},_this.playTime)
		},
		//鼠标移入移出事件
	mouseoverout:function(){
		var _this=this;
		this.bigbox.onmouseover=function(){
			clearInterval(_this.play);
			// for (var i = 0; i < 2; i++) {
				// _this.achange[i].style.opacity=1;
				// _this.achange[i].style.filter='alpha(opacity=100)';
			// };
		};
		this.bigbox.onmouseout=function(){
			_this.autoplay();

		};

		for (var i = 0; i < this.count; i++) {
			this.numlist[i].index=i;
			this.numlist[i].onmouseover=function(){
				if (_this.index==this.index) {}else{

				_this.imgshow(this.index);
					
				}
			};
		};
	},


	};
	

/*	弹窗控制	*/

		$(document).ready(function(){
			var timer=null;
			var h;

			/*	右边小窗口位置控制	*/

			var siderighttop=window.screen.availHeight;
			var siderighth=$(".sideright").height();
			var sidelefth=$(".sideleft").height();
			var sidelefttop=(siderighttop-sidelefth)/2;
			var positop=(siderighttop-siderighth)/2;
			$(".sideright").css("top",positop+"px");
			$(".sideleft").animate({"top":sidelefttop+"px",
									"left":0+"px",
									opacity:1,
									},1000);
			/*	左边小窗口位置控制	*/
			timer=setInterval(function(){
			// $(document).scroll(function(){
				$(".sideleft").css("top",h+sidelefttop+"px");
			//})
			h=$(document).scrollTop();
		},1);

			/*	修改	*/
			$(".l_addalert").click(function(){
				$(".l_groupcen_passokche").show();
				
			})
			$(".alert_exitche").click(function(){
				$(".l_groupcen_passokche").hide();
			})
			$(".l_alert_sub").click(function(){
				$(".l_groupcen_passokche").hide();

			})
		})
