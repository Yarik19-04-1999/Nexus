import { Suspense } from 'react'
import { InvitesTable } from '@/components/admin/InvitesTable'
import { Spinner } from '@/components/ui/Spinner'

export default function AdminPage() {
  return (
    <main className="min-h-screen bg-gray-50 p-4 sm:p-8">
      <div className="max-w-4xl mx-auto">
        <Suspense fallback={<div className="flex justify-center py-12"><Spinner /></div>}>
          <InvitesTable />
        </Suspense>
      </div>
    </main>
  )
}
