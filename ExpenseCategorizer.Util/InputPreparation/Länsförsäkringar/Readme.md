
Instructions in Swedish as LF is a swedish bank.

Förberedelser som behövs för Länsförsäkringar.

1. Logga in och ta fram ditt kontoutdrag så långt som du vill ha med.
2. Kör följande i console (i Chrome):
	```javascript
	document.querySelector("[aria-label=Transaktionshistorik]").outerHTML
	```
	
3. Kopiera svaret (undantaget citattecknen) och klistra in det i en textfil som döps till "lf.html" (spara som UTF-8).
4. Öppna lf.html i Chrome och kör följande i consolen:
	```javascript
	document.querySelectorAll(".reset-table").forEach(function(e) { e.parentNode.removeChild(e); });
	document.querySelectorAll("button").forEach(function(e) { e.parentNode.removeChild(e); });
	document.querySelectorAll("tr > td > .link-complex-target").forEach(function(e) { 
		if(e.nextSibling.innerText == '') return;
		 
		if(e.innerText === 'Kortköp' || e.innerText === 'Överföring' || e.innerText === 'Betalning')
			e.parentNode.removeChild(e);
		else {
			e.nextSibling.innerText = e.innerText + ' ' + e.nextSibling.innerText;
		 	e.parentNode.removeChild(e); 
		}
	});
	document.querySelectorAll("tr > th").forEach(function(e) { e.parentNode.removeChild(e); });
	document.querySelectorAll(".spinner").forEach(function(e) { var tr = e.closest('tr'); tr.parentNode.removeChild(tr); });
	document.querySelectorAll(".table-row-collapse").forEach(function(e) { e.parentNode.removeChild(e); });
	```
5. Stäng consolen och kopiera alltihop.
6. Klistra in i Excel och spara som "Text (tab delimited) (*.txt)".
7. Öppna filen i notepad och spara om den som utf-8 eftersom Excel inte gör det rätt.
8. Klart! Kört ExpenseCategorizer med filen och kategorimappningen.