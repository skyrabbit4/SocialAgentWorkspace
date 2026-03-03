import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SocialPost } from '../models/social-post.model';

@Injectable({
  providedIn: 'root',
})
export class PostService {
  private apiUrl='http://localhost:5221/api';
  constructor(private http:HttpClient)
  {
  }
  getHistory():Observable<SocialPost[]>{
    return this.http.get<SocialPost[]>(`${this.apiUrl}/history`)
  }

  postTweet(content:string):Observable<any>{
    return this.http.post(`${this.apiUrl}/tweet?tweetText=${content}`,{})
  }
}
