import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { ConnectionStatusComponent } from './ui/connection-status/connection-status.component';
import { ConnectionService } from './services/connection.service';
import { MenuService } from './services/menu.service';


@NgModule({
  declarations: [
    AppComponent,
    ConnectionStatusComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    NgbModule
  ],
  providers: [ConnectionService, MenuService],
  bootstrap: [AppComponent]
})
export class AppModule { }
