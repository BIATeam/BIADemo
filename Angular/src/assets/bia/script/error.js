if (!String.prototype.includes) {
  String.prototype.includes = function (search, start) {
    if (typeof start !== 'number') {
      start = 0;
    }

    if (start + search.length > this.length) {
      return false;
    } else {
      return this.indexOf(search, start) !== -1;
    }
  };
}
function closest(el, predicate) {
  if (el == null || el == document) {
    return null;
  }
  return predicate(el) ? el : el && closest(el.parentNode, predicate);
}
function getParameterByName(name, url) {
  if (!url) {
    var myDiv = document.getElementById('MyError');
    var dialog = closest(myDiv, function (el) {
      return el.className.includes('BiaNetDialogDiv');
    });
    //alert("dialog: " + dialog);
    if (dialog == null) url = window.location.href.replace('#', '');
    else {
      url = BIA.Net.Dialog.DialogDiv.GetParentDialogDiv(
        $('MyError')
      ).urlCurrent;
    }
    //alert("dialog.dialogUrlCurrent: " + dialog.dialogUrlCurrent);

    //alert("url: " + url);
  }
  name = name.replace(/[\[\]]/g, '\\$&');
  var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
    results = regex.exec(url);
  if (!results) return null;
  if (!results[2]) return '';
  return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

//$(document).ready()
(function () {
  var num = getParameterByName('num');

  if (num === '404') {
    numError.innerText = '404';
    titleError.innerText = 'Page Not Found';
    descError.innerText =
      'Sorry, but the page you are looking for has not been found. Please contact your Helpdesk.';
  } else if (num === '403') {
    numError.innerText = '403';
    titleError.innerText = 'Forbidden';
    descError.innerText =
      'You are currently not allowed to access ot this element.';
  } else if (num === '401') {
    numError.innerText = '401';
    titleError.innerText = 'Unauthorized';
    descError.innerText = 'You are currently not allowed to access..';
  } else {
    numError.innerText = num;
  }

  var width = 15;
  function frame() {
    if (width >= 95) {
      document.getElementById('myBarBorder').className += ' animated hinge';
      clearInterval(id);
    } else {
      width++;
      document.getElementById('myBar').style.width = width + '%';
    }
  }
  var id = setInterval(frame, 100);
})();
