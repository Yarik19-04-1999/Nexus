import { AdminLanguageProvider } from '@/components/admin/AdminLanguageContext'

export default function AdminLayout({ children }: { children: React.ReactNode }) {
  return <AdminLanguageProvider>{children}</AdminLanguageProvider>
}
