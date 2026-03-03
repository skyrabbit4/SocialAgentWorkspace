import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import type { SocialPost } from '../../models/social-post.model';

import { PostService } from '../../services/post'; 

@Component({
  selector: 'app-history',
  standalone: true,           
  imports: [CommonModule],    
  templateUrl: './history.html',
  styleUrls: ['./history.css'],
})
export class HistoryComponent implements OnInit {

  posts: SocialPost[] = [];
  
  constructor(private postService: PostService) {}

  ngOnInit(): void {
    this.refreshHistory();
  }

  refreshHistory(): void {
   this.postService.getHistory().subscribe({
    next: (data: SocialPost[]) => {
      this.posts = data;
   },
   error: (err: any) => console.error('Error Fetching History', err)
  });
  }
}