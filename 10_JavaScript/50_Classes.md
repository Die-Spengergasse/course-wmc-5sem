# Klassen

Im vorigen Kapitel haben wir gesehen, dass wir über Functions und Prototypes bereits Features,
die wir aus Klassen kennen, umsetzen können.

In ECMAScript 6 (2015) wurde eine spezielle Syntax für Klassen entwickelt. Dies hatte 2 Gründe:
- Das Wissen über Klassen ist aus anderen OOP Sprachen wie Java oder C# weit verbreitet.
- Eine Klasse erzwingt den Einsatz von *new* bei der Instanzierung.

Auf https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes sind die 
Möglichkeiten der Klassendefinition sehr gut dargestellt. Mit dem Wissen aus Java oder C# fällt es 
besonders leicht, die einzelnen Bestandteile wieder zu erkennen. Es ist jedoch folgendes zu
beachten:
- Einige Features wie private und public field declarations sind (noch) nicht standardisiert,
  sondern im Status eines proposals (https://github.com/tc39/proposal-class-fields).
- Beachte daher die Browserkompatibilität am Ende der MDN Seite
  (https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes#browser_compatibility).



Als praktisches Beispiel können wir einen Blog mit 2 Klassen implementieren. Es benutzt nur
Features aus dem ECMAScript 6 Standard, d. h. in jedem gängigen Browser direkt ausführbar:

```javascript
class User {
    constructor(firstname, lastname, email) {
        this.firstname = firstname;
        this.lastname = lastname;
        this.email = email;
    }
    
    get id() { return this.email; }
}

class Post {
    constructor(user, title) {
        this.user = user;
        this.title = title;
        this.ratings = [];
        this.comments = [];
    }

    addComment(user, comment) {
        // Important: operator precedence. Write !(a instanceof class)
        if (!(user instanceof User)) { throw new TypeError('user is not an instance of User.'); }
        if (typeof comment !== "string") { throw new TypeError('comment is not a string of User.'); }

        this.comments.push({ date: new Date(), user: user, comment: comment });
    }

    tryAddRating(user, rating) {
        if (!(user instanceof User)) { throw new TypeError('user is not an instance of User.'); }
        if (!isFinite(rating = Number(rating))) { throw new TypeError('rating is not a number.'); }

        if (this.ratings.find(r => r.user.id == user.id)) { return false; }
        this.ratings.push({ user: user, rating: rating });
        return true;
    }

    get commentsCount() {
        return this.comments.length;
    }

    get averageRating() {
        if (!this.ratings.length) { return; }
        return this.ratings.reduce((prev, current) => prev + current.rating, 0) / this.ratings.length;
    }

    getCommentsFromUser(user) {
        if (!(user instanceof User)) { throw new TypeError('user is not an instance of User.'); }

        return this.comments.filter(c => c.user.id == user.id).length;
    }
}

const post = new Post(new User("Max", "Mustermann", "max@mail.at"));
const commentator1 = new User("Sophie", "Musterfrau", "sophie@mail.at");
const commentator2 = new User("Tobias", "Eifrig", "tobias@mail.at");

post.addComment(commentator1, "First comment.");
post.addComment(commentator1, "Second comment.");
post.addComment(commentator2, "Third comment.");
post.tryAddRating(commentator1, 4);
post.tryAddRating(commentator1, 3);
post.tryAddRating(commentator2, 3);
console.log(`Number of comments: ${post.commentsCount}`);
console.log(`Average rating: ${post.averageRating}`);
console.log(`${commentator1.id} has written ${post.getCommentsFromUser(commentator1)} comments.`);
```

Wir erkennen in diesem Code einige Neuerungen:

- Mit **instanceof** können wir prüfen, ob ein Objekt eine Instanz einer bestimmten Klasse ist.
- Mit *get id()* können wir ein read-only Property realisieren. Der Wert kann dann mit *p.id*
  (ohne Klammern) aufgerufen werden.
- Im Konstruktor weisen wir mit this.variable die Membervariablen zu. Dies können wir auch
  in jeder anderen Methode machen. Diese Variablen haben natürlich vor ihrer Zuweisung den
  Wert *undefined*.

## Übung

Da wir gleich das Modulkonzept nutzen wollen, befindet sich die Übung am Ende des nächsten Kapitels
(Module).