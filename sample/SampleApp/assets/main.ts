import domready from 'domready';

domready(() => {
  var body = document.body;
  var title = document.createElement('h1');
  title.innerText = 'I am from typescript (via webpack)';
  document.title = title.innerText;
  body.appendChild(title);
});
