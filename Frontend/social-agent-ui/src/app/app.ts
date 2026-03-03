import { Component, OnInit, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  backendMessage: string = "Waiting for backend";
  userQuery: string = "";
  isLoading: boolean = false;

  private http = inject(HttpClient);

  ngOnInit() {
    this.checkBackendStatus();
  }

  checkBackendStatus() {
    this.http.get<any>('http://localhost:5221/api/status')
      .subscribe({
        next: (response) => {
          this.backendMessage = response.message;
        },
        error: (error) => {
          console.error('API Error', error);
          this.backendMessage = 'failed to connect to backend';
        }
      });
  }

 sendToAi() {
    if (!this.userQuery) return;

    this.isLoading = true;
    this.backendMessage = 'Agent is thinking...';

    // Fixed: capital 'P' in userPrompt
    this.http.get<any>(`http://localhost:5221/api/chat?userPrompt=${this.userQuery}`)
    .subscribe({
      next:(response)=>{
        this.backendMessage=response.answer;
        this.isLoading=false;
        this.userQuery="";
      },
      error:(err)=>{
        this.backendMessage='Error: check your dotnet terminal'
        this.isLoading=false;
      }
    });
  }

  postToX() {
    // Fixed: lowercase 'thinking'
    if(!this.backendMessage || this.backendMessage.includes('Waiting') || this.backendMessage.includes('thinking'))
    {
      alert("Please wait for a valid AI Response");
      return;
    }
    
    this.isLoading=true;
    this.http.post<any>(`http://localhost:5221/api/tweet?tweetText=${encodeURIComponent(this.backendMessage)}`,{})
    .subscribe({
      next:(response)=>{
        alert('Success! Tweet Posted. ID:'+response.tweetId);
        this.isLoading=false;
      },
      error:(err)=>{
        console.error(err);
        alert('Failed to post tweet. Check your .NET terminal for errors.');
        this.isLoading = false;
      }
    })
  }

}