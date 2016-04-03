import {bootstrap}    from "angular2/platform/browser";
import {AppComponent} from "./app.component";
bootstrap(AppComponent, [])
    .then((success: any) => console.log("Bootstrap success"))
    .catch((error: Error) => console.log(error));
