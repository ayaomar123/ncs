export interface Appeal {
  id: string;
  title: string;
  slug: string;
  summary: string;
  description: string;
  countryTag?: string | null;
  isUrgent: boolean;
  targetAmount: number;
  raisedAmount: number;
  coverImageUrl?: string | null;
  galleryUrls: string[];
  status: number;
  createdAt: string;
  publishedAt?: string | null;
}
