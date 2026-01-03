export interface BlogPost {
  id: string;
  title: string;
  slug: string;
  excerpt: string;
  content: string;
  coverImageUrl?: string | null;
  tags: string[];
  createdAt: string;
  publishedAt?: string | null;
  isPublished: boolean;
}
