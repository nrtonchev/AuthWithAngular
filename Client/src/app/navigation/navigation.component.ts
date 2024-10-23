import { Component, DoCheck } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css'
})
export class NavigationComponent implements DoCheck {
  isUserAuthenticated: boolean = false;
  
  ngDoCheck(): void {
    const token = localStorage.getItem("jwt");
    if (token){
      this.isUserAuthenticated = true;
    }
    else{
      this.isUserAuthenticated = false;
    }
  }

  logOut() {
    localStorage.removeItem("jwt");
  }
}
