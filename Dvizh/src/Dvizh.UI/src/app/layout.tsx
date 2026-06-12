import type { Metadata } from 'next'
import { Geist } from 'next/font/google'
import { QueryProvider } from '@/components/providers/QueryProvider'
import './globals.css'

const geist = Geist({ subsets: ['latin'], variable: '--font-geist' })

export const metadata: Metadata = {
  title: 'Dvizh',
  description: 'Invite system',
}

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="ru" className={`${geist.variable} h-full antialiased`}>
      <body className="min-h-full font-[family-name:var(--font-geist)]">
        <QueryProvider>{children}</QueryProvider>
      </body>
    </html>
  )
}
