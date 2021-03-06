import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SocketServiceService } from 'src/socket-service.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public uname = '';
  public uemail = '';
  public isUserLogged = false;

  constructor(private socketIOService: SocketServiceService,
    private router: Router) {
  }
  ngOnInit(): void {
  this.GetSession();
  }
 // tslint:disable-next-line:typedef
 GetSession(){
  this.socketIOService.GetSession()
  .subscribe((data: any) => {
    
    console.log('sesssss: ', data);
  });
 }
  Login() {
    if (this.uname != '') {
      sessionStorage.setItem('username', this.uname);
      this.isUserLogged = true;
      this.router.navigate(['/chat']);
    }
  }

}
