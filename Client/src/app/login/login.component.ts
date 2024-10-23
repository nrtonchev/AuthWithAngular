import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginModel } from '../interfaces/login.model';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { AuthResponse } from '../interfaces/authResponse.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent{
  http: HttpClient = inject(HttpClient);
  router: Router = inject(Router);
  invalidLogin: boolean = true;

  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required)
  });

  onSubmit() {
    if (this.loginForm.valid){
      const loginModel: LoginModel = {
        email: this.loginForm.value.username!,
        password: this.loginForm.value.password!
      };

      this.http.post<AuthResponse>("https://localhost:5001/api/Auth/authenticate",
        loginModel, 
        { headers: new HttpHeaders({"Content-Type": "application/json"}) }
      )
      .subscribe({
        next: (response: AuthResponse) => {
          const token = response.jwtToken;
          localStorage.setItem("jwt", token);
          this.invalidLogin = false;
          this.router.navigate(["/"]);
        },
        error: (err: HttpErrorResponse) => this.invalidLogin = true
      })
    }
  }
}
