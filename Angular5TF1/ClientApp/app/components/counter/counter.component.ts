import { Component } from '@angular/core';
import { Http, RequestOptions, Headers, Response } from '@angular/http';
import { RequestOptionsArgs } from '@angular/http/src/interfaces';

@Component({
    selector: 'counter',
    templateUrl: './counter.component.html',
    styleUrls: ['./counter.styles.css']
})
export class CounterComponent {

    public constructor(private http: Http) {

    }

    public isDisabled = false;
    public InsecureOutput: string;
    public Login: string;
    public Password: string;
    public IsAdmin: boolean;
    public AllUsers: string;
    public NewLogin: string;
    public NewPassword: string;
    public CreateInsecureOutput: string;
    ///////////////////////////////////

    public SecureOutput: string;
    public LoginSecure: string;
    public PasswordSecure: string;
    public IsAdminSecure: boolean;
    public AllUsersSecure: string;
    public NewLoginSecure: string;
    public NewPasswordSecure: string;
    public CreateSecureOutput: string;



    public tryCreateInsecure() {
        this.isDisabled = true;
        this.CreateInsecureOutput = '';
        const url = "http://localhost:2873/api/SIS/createInsecure";
        let obj: NamePassword = { Name: this.NewLogin, Password: this.NewPassword };
        let headersMy = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headersMy });
        this.http.post(url, obj, options)
            .subscribe((res: Response) => {
                if (res.ok) {
                    let resp = res.json();
                    this.CreateInsecureOutput = JSON.stringify(resp);
                }
                this.isDisabled = false;
            }, err => {
                this.CreateInsecureOutput = JSON.stringify(err.json());

                this.isDisabled = false;
            });
    }

    public tryCreateSecure() {
        this.isDisabled = true;
        this.CreateSecureOutput = '';
        const url = "http://localhost:2873/api/SIS/createSecure";
        let obj: NamePassword = {
            Name: this.NewLoginSecure,
            Password: this.NewPasswordSecure
        };
        let headersMy = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headersMy });
        debugger;
        this.http.post(url, obj, options)
            .subscribe((res: Response) => {
                if (res.ok) {
                    let resp = res.json();
                    this.CreateSecureOutput = JSON.stringify(resp);
                }
                this.isDisabled = false;
            }, err => {
                this.CreateSecureOutput = JSON.stringify(err.json());

                this.isDisabled = false;
            });
    }


    public tryLoginInsecure() {
        this.isDisabled = true;;
        this.IsAdmin = false;
        this.InsecureOutput = '';
        this.AllUsers = '';
        const url = "http://localhost:2873/api/SIS/tryLoginNotSecure";
        let obj: NamePassword = { Name: this.Login, Password: this.Password };
        let headersMy = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headersMy });
        this.http.post(url, obj, options)
            .subscribe((res: Response) => {
                if (res.ok) {
                    let resp = res.json();
                    this.IsAdmin = resp.isAdmin;
                    this.InsecureOutput = resp.message;
                }
                this.isDisabled = false;
            }, err => {
                this.InsecureOutput = err.json();

                this.isDisabled = false;
            });
    }

    public tryLoginSecure() {
        this.isDisabled = true;
        this.IsAdminSecure = false;
        this.SecureOutput = '';
        this.AllUsersSecure = '';
        const url = "http://localhost:2873/api/SIS/tryLoginSecure";
        let obj: NamePassword = { Name: this.LoginSecure, Password: this.PasswordSecure };
        let headersMy = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headersMy });
        this.http.post(url, obj, options)
            .subscribe((res: Response) => {

                if (res.ok) {
                    let resp = res.json();
                    this.IsAdminSecure = resp.isAdmin;
                    this.SecureOutput = resp.message;
                }
                this.isDisabled = false;
            }, err => {
                this.SecureOutput = JSON.stringify(err.json());

                this.isDisabled = false;
            });
    }

    public getAllUsers() {
        this.isDisabled = true;
        const url = "http://localhost:2873/api/SIS/getAllInsecure";
        let headersMy = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headersMy });
        this.http.get(url, options)
            .subscribe((res: Response) => {
                if (res.ok) {
                    let resp = res.json();
                    this.AllUsers = JSON.stringify(resp);
                }
                this.isDisabled = false;
            }, err => {
                this.AllUsers = err.json();

                this.isDisabled = false;
            });
    }

    public getAllUsersSecure() {
        this.isDisabled = true;
        const url = "http://localhost:2873/api/SIS/getAllSecure";
        let headersMy = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headersMy });
        this.http.get(url, options)
            .subscribe((res: Response) => {
                if (res.ok) {
                    let resp = res.json();
                    this.AllUsersSecure = JSON.stringify(resp);
                }
                this.isDisabled = false;
            }, err => {
                this.AllUsersSecure = err.json();

                this.isDisabled = false;
            });
    }
}

interface NamePassword {
    Name: string
    Password: string
}
