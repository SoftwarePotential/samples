<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Refresher.ascx.cs" Inherits="Slps.Distributor.Web.Views.Shared.Controls.Refresher" %>

<script type="text/javascript">
	$(document).ready(function() {

		var refresher = $('div#refresher');

		refresher.refreshOptionIndex = refresherIndexFromCookie();
	
		refresher.refreshOptions = { options: [
				{ "name": "Off",        "interval":             0 },
				{ "name": "10 Seconds", "interval":     10 * 1000 },
				{ "name": "30 Seconds", "interval":     30 * 1000 },
				{ "name": "1 Minute",   "interval": 1 * 60 * 1000 },
				{ "name": "5 Minutes",  "interval": 5 * 60 * 1000 }
				] };
		
		setStateThing($('#refresher > a')[0], refresher);

		$('#refresher > a').click(function() {

			// Loop through the options, find selected
			refresher.refreshOptionIndex = (refresher.refreshOptionIndex + 1) % refresher.refreshOptions.options.length;

			// Set in cookie (which expires when browser closes)
			refresherIndexFromCookie(refresher.refreshOptionIndex);

			setStateThing(this, refresher);

			return false;
		});
	});

	function setStateThing(ctrl, refresher) {

		var selected = refresher.refreshOptions.options[refresher.refreshOptionIndex];
		
		if (refresher.refreshIntervalId !== undefined)
			window.clearInterval(refresher.refreshIntervalId);

		ctrl.innerHTML = selected.name;

		if (selected.interval > 0) {
			ctrl.className = "toggle_option_on";
			refresher.refreshIntervalId = window.setInterval(refresher.attr('onrefresh'), selected.interval);
		}
		else {
			ctrl.className = "toggle_option_off";
			refresher.refreshIntervalId = undefined;
		}
	}

	function refresherIndexFromCookie(value) {
		
		if (typeof value != 'undefined') {
			// Set cookie value
			document.cookie = 'refresher=' + value;
		}
		else {
			// Get cookie value
			var cookieValue = '0';

			if (document.cookie && document.cookie != '') {
				var cookies = document.cookie.split(';');
				for (var i = 0; i < cookies.length; i++) {
					var cookie = jQuery.trim(cookies[i]);
					if (cookie.substring(0, 'refresher='.length) == ('refresher=')) {
						cookieValue = decodeURIComponent(cookie.substring('refresher='.length));
						break;
					}
				}
			}

			return parseInt(cookieValue, 10);
		}
	}
</script>
<div id="refresher" onrefresh="<%=OnTimerTick %>">Automatic refresh: <a class="toggle_option_off">Off</a></div>